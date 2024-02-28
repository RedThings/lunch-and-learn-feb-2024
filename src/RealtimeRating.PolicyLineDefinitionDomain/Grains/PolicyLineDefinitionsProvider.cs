using Orleans.Runtime;
using RealtimeRating.PolicyLineDefinitionDomain.Dtos;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;
using RealtimeRating.PolicyLineDefinitionDomain.State;

namespace RealtimeRating.PolicyLineDefinitionDomain.Grains;

public class PolicyLineDefinitionsProvider(
    [PersistentState(stateName: nameof(PolicyLineDefinitionsState), storageName: PolicyLineDefinitionDomainConstants.StorageName)]
    IPersistentState<PolicyLineDefinitionsState> persistentState) : IProvideAllPolicyLineDefinitions
{
    public async Task<IReadOnlyCollection<PolicyLineDefinition>> Ask(GetAllPolicyLineDefinitions message)
    {
        if (persistentState.State.Definitions != null)
        {
            return persistentState.State.Definitions;
        }

        var definitions = BuildDefinitions();

        persistentState.State = new PolicyLineDefinitionsState
        {
            Definitions = definitions
        };

        await persistentState.WriteStateAsync();

        return definitions;
    }

    public Task<IReadOnlyCollection<ValidatedQuestion>> Ask(ValidateAnswers message)
    {
        // todo: something better than this OBVIOUSLY - each pld should be in a grain, have it's own validation etc etc, this is just for messing about

        if (persistentState.State.Definitions == null)
        {
            throw new Exception($"No definitions exist - the {nameof(Ask)}/{nameof(GetAllPolicyLineDefinitions)} method must be called first");
        }

        var pld = persistentState.State.Definitions.SingleOrDefault(x => x.Code == message.PolicyLineDefinitionCode)
                  ?? throw new Exception($"{message.PolicyLineDefinitionCode} was not found");

        var output = pld.Questions.Select(staticQuestion =>
        {
            var answer = default(string?);
            var validationError = default(string?);

            var answerDto = message.Answers.SingleOrDefault(x => x.Code == staticQuestion.Code);

            if (answerDto == null)
            {
                validationError = "Question does not exist";
            }
            else
            {
                answer = answerDto.Value;

                validationError = answerDto.Code switch
                {
                    "nm_prfx" => answer == "Miss" ? "Value cannot be 'Miss'" : default,
                    "nm2" => string.IsNullOrWhiteSpace(answer) ? "Must have a value" : default,
                    _ => validationError
                };
            }

            return new ValidatedQuestion
            {
                Code = staticQuestion.Code,
                Label = staticQuestion.Label,
                Type = staticQuestion.Type,
                Options = staticQuestion.Options,
                Answer = answer,
                ValidationError = validationError
            };
        }).ToArray();

        return Task.FromResult((IReadOnlyCollection<ValidatedQuestion>) output);
    }

    private static PolicyLineDefinition[] BuildDefinitions()
    {
        // fetch this at start up and put in a provider - or fetch on demand once in this grain
        var policyTypesFromServiceCode = new[]
        {
            ("UKMO", "UK Motor"),
            ("UKHO", "UK Home"),
            ("UKCV", "UK Caravan"),
            ("UKGF", "UK Golf"),
            ("UKVN", "UK Van")
        };

        var definitions = policyTypesFromServiceCode.Select(x => new PolicyLineDefinition
        {
            Code = x.Item1,
            Name = x.Item2,
            Questions = BuildQuestions(x.Item1)
        }).ToArray();

        return definitions;
    }

    private static List<Question> BuildQuestions(string code)
    {
        // todo: initialize at startup or lazy load here

        var questions = new List<Question>
        {
            new()
            {
                Code = "nm_prfx",
                Label = "Prefix",
                Type = QuestionType.DropDown,
                Options = new[]
                {
                    new DropDownOption { Name = "Miss", Value = "Miss" },
                    new DropDownOption { Name = "Mrs", Value = "Mrs" },
                    new DropDownOption { Name = "Mr", Value = "Mr" },
                    new DropDownOption { Name = "Dr.", Value = "Dr." }
                }
            },
            new()
            {
                Code = "nm1",
                Label = "First Name",
                Type = QuestionType.SingleLineText
            },
            new()
            {
                Code = "nm2",
                Label = "Last Name",
                Type = QuestionType.SingleLineText
            },
            new()
            {
                Code = "pc",
                Label = "Postal/Zip Code",
                Type = QuestionType.SingleLineText
            },
            new()
            {
                Code = "dob",
                Label = "Date of Birth",
                Type = QuestionType.SingleLineText
            }
        };

        switch (code)
        {
            case "UKMO":
                {
                    questions.Add(new Question
                    {
                        Code = "reg",
                        Label = "Car reg.",
                        Type = QuestionType.SingleLineText
                    });
                    break;
                }
            case "UKHO":
                {
                    questions.Add(new Question
                    {
                        Code = "val",
                        Label = "Home value if owned",
                        Type = QuestionType.SingleLineText
                    });
                    break;
                }
            case "UKCV":
                {
                    questions.Add(new Question
                    {
                        Code = "val",
                        Label = "Caravan value if owned",
                        Type = QuestionType.SingleLineText
                    });
                    break;
                }
            case "UKGF":
                {
                    questions.Add(new Question
                    {
                        Code = "val",
                        Label = "Value of clubs",
                        Type = QuestionType.SingleLineText
                    });
                    break;
                }
            case "UKVN":
                {
                    questions.Add(new Question
                    {
                        Code = "reg",
                        Label = "Van reg.",
                        Type = QuestionType.SingleLineText
                    });
                    break;
                }
        }

        return questions;
    }
}