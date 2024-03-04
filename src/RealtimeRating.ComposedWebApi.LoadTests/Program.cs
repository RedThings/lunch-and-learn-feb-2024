using System.Net.Mime;
using System.Text;
using System.Text.Json;
using NBomber.Contracts;
using NBomber.CSharp;
using RealtimeRating.Composition.NewQuote;
using RealtimeRating.Composition.Rating;
using RealtimeRating.Composition.RiskDataCapture;

var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
var httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7270") };

RunTests();
Console.ReadLine();
return;

/*************************************************************************************8*/

void RunTests()
{
    Console.WriteLine("How many concurrent users?");
    var howMany = int.Parse(Console.ReadLine() ?? string.Empty);

    Console.WriteLine("Interval milliseconds?");
    var intervalSeconds = int.Parse(Console.ReadLine() ?? string.Empty);

    Console.WriteLine("How long to run for (seconds)?");
    var howLongSeconds = int.Parse(Console.ReadLine() ?? string.Empty);

    Console.WriteLine();
    Console.WriteLine($"Hit enter to run the test for {howMany} users, for {howLongSeconds} seconds, with an interval of {intervalSeconds} seconds");
    Console.ReadLine();

    var runner = NBomberRunner
        .RegisterScenarios(
            BuildScenario(howMany, TimeSpan.FromMilliseconds(intervalSeconds), TimeSpan.FromSeconds(howLongSeconds), waitForResults: false),
            BuildScenario(howMany, TimeSpan.FromMilliseconds(intervalSeconds), TimeSpan.FromSeconds(howLongSeconds), waitForResults: true)
        );

    runner.Run();

    Console.WriteLine();
}

ScenarioProps BuildScenario(int rate, TimeSpan interval, TimeSpan during, bool waitForResults)
{
    var title = waitForResults ? "Start rating and wait for all results" : "Start rating";

    var scenario = Scenario.Create(title, async _ =>
    {
        try
        {
            var customerId = Guid.NewGuid(); // need a unique customer here so CustomerQuote grain doesn't experience contention

            //
            var newQuoteHttpResponse = await httpClient.GetAsync($"/new-quote?customer_id={customerId}").ConfigureAwait(false);
            var newQuoteJson = await newQuoteHttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var newQuoteResponse = JsonSerializer.Deserialize<NewQuoteResponse>(newQuoteJson, jsonOptions) ?? throw new Exception("Could not deserialize");

            var pldIndex = Faker.RandomNumber.Next(0, newQuoteResponse.PolicyLineDefinitions.Length - 1);
            var pldCode = newQuoteResponse.PolicyLineDefinitions.ElementAt(pldIndex).Code;

            newQuoteHttpResponse.Dispose();
            //

            //
            var submitNewQuoteRequest = new SubmitNewQuoteRequest { Name = Faker.Name.FullName() };
            var submitNewQuoteRequestJson = JsonSerializer.Serialize(submitNewQuoteRequest);
            var submitNewQuoteRequestContent = new StringContent(submitNewQuoteRequestJson, Encoding.UTF8, MediaTypeNames.Application.Json);
            var submitNewQuoteHttpResponse = await httpClient.PostAsync($"/new-quote?customer_id={customerId}", submitNewQuoteRequestContent).ConfigureAwait(false);
            var submitNewQuoteJson = await submitNewQuoteHttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var submitNewQuoteResponse = JsonSerializer.Deserialize<SubmitNewQuoteResponse>(submitNewQuoteJson, jsonOptions) ?? throw new Exception("Could not deserialize");

            var quoteId = submitNewQuoteResponse.QuoteId;
            var riskVariationId = submitNewQuoteResponse.RiskVariationId;

            submitNewQuoteRequestContent.Dispose();
            submitNewQuoteHttpResponse.Dispose();
            //

            //
            var newRiskDataCaptureHttpResponse = await httpClient.GetAsync($"/risk-data-capture?customer_id={customerId}&quote_id={quoteId}&risk_variation_id={riskVariationId}&policy_line_definition_code={pldCode}").ConfigureAwait(false);
            var newRiskDataCaptureJson = await newRiskDataCaptureHttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var newRiskDataCaptureResponse = JsonSerializer.Deserialize<NewRiskDataCaptureResponse>(newRiskDataCaptureJson, jsonOptions) ?? throw new Exception("Could not deserialize");

            newQuoteHttpResponse.Dispose();
            //

            //
            var answers = newRiskDataCaptureResponse.Questions.Select(
                q =>
                {
                    return q.Code switch
                    {
                        "nm_prfx" => new KeyValuePair<string, string>(q.Code, "Mrs"),
                        "nm2" => new KeyValuePair<string, string>(q.Code, Faker.Name.Last()),
                        _ => new KeyValuePair<string, string>(q.Code, Faker.RandomNumber.Next(0, 1000000).ToString())
                    };
                }).ToArray();

            var url = waitForResults
                          ? $"/risk-data-capture?customer_id={customerId}&quote_id={quoteId}&risk_variation_id={riskVariationId}&policy_line_definition_code={pldCode}"
                          : $"/risk-data-capture?do_not_rate=true&customer_id={customerId}&quote_id={quoteId}&risk_variation_id={riskVariationId}&policy_line_definition_code={pldCode}";
            var submitNewRiskDataCaptureRequestContent = new FormUrlEncodedContent(answers);
            var submitNewRiskDataCaptureHttpResponse = await httpClient.PostAsync(url, submitNewRiskDataCaptureRequestContent).ConfigureAwait(false);
            var submitNewRiskDataCaptureJson = await submitNewRiskDataCaptureHttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var startRatingResponse = JsonSerializer.Deserialize<StartRatingResponse>(submitNewRiskDataCaptureJson, jsonOptions) ?? throw new Exception("Could not deserialize");

            var ratingSessionId = startRatingResponse.RatingSessionId;

            submitNewRiskDataCaptureRequestContent.Dispose();
            submitNewRiskDataCaptureHttpResponse.Dispose();
            //

            if (!waitForResults)
            {
                return Response.Ok();
            }

            //
            var counter = 0;

            while (counter < 10)
            {
                using var getRatingResultsHttpResponse = await httpClient.GetAsync($"/rating-results?customer_id={customerId}&rating_session_id={ratingSessionId}&quote_id={quoteId}&risk_variation_id={riskVariationId}&policy_line_definition_code={pldCode}").ConfigureAwait(false);
                var getRatingResultsJson = await getRatingResultsHttpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                var getRatingResultsResponse = JsonSerializer.Deserialize<DisplayRatingResultsResponse>(getRatingResultsJson, jsonOptions) ?? throw new Exception("Could not deserialize");

                var finished = getRatingResultsResponse.FinishedRating;

                if (finished)
                {
                    return Response.Ok();
                }

                counter++;

                await Task.Delay(1000);
            }

            //
            return Response.Fail();
        }
        catch (Exception ex)
        {
            return Response.Fail(ex);
        }
    })
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(Simulation.Inject(rate, interval, during));

    return scenario;
}


