using RealtimeRating.Composition;
using RealtimeRating.Composition.RiskDataCapture;
using RealtimeRating.PolicyLineDefinitionDomain.Dtos;
using RealtimeRating.PolicyLineDefinitionDomain.Grains;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;

namespace RealtimeRating.PolicyLineDefinitionDomain.CompositionParticipators;

public class SubmitNewRiskDataCaptureParticipator(IGrainFactory grainFactory)
    : IParticipateInViewModelComposition<SubmitNewRiskDataCaptureRequest, SubmitNewRiskDataCaptureResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(SubmitNewRiskDataCaptureRequest request, SubmitNewRiskDataCaptureResponse response)
    {
        var service = grainFactory.GetGrain<IProvideAllPolicyLineDefinitions>(Guid.Empty);

        var questionsWithValidation = await service.Ask(new ValidateAnswers
        {
            PolicyLineDefinitionCode = request.PolicyLineDefinitionCode,
            Answers = request.Answers.Select(x => new Answer { Code = x.Key, Value = x.Value }).ToArray()
        });

        response.Questions = questionsWithValidation.Select(
            x => new QuestionForRiskDataCapture
            {
                Code = x.Code,
                Label = x.Label,
                Type = (QuestionTypeForRiskDataCapture) x.Type,
                Options = x.Options?.Select(y => new DropDownOptionForRiskDataCapture { Name = y.Name, Value = y.Value }).ToArray(),
                Answer = x.Answer,
                ValidationError = x.ValidationError
            }).ToArray();
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}