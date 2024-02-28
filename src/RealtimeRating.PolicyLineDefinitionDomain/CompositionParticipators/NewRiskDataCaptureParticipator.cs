using RealtimeRating.Composition;
using RealtimeRating.Composition.RiskDataCapture;
using RealtimeRating.PolicyLineDefinitionDomain.Grains;
using RealtimeRating.PolicyLineDefinitionDomain.Messages;

namespace RealtimeRating.PolicyLineDefinitionDomain.CompositionParticipators;

public class NewRiskDataCaptureParticipator(IGrainFactory grainFactory) : IParticipateInViewModelComposition<NewRiskDataCaptureRequest, NewRiskDataCaptureResponse>
{
    public int ExecutionOrder => 0;

    public async Task Participate(NewRiskDataCaptureRequest request, NewRiskDataCaptureResponse response)
    {
        var service = grainFactory.GetGrain<IProvideAllPolicyLineDefinitions>(Guid.Empty);
        var allPolicyLineDefinitions = await service.Ask(new GetAllPolicyLineDefinitions());
        var policyLineDefinition = allPolicyLineDefinitions.SingleOrDefault(x => x.Code == request.PolicyLineDefinitionCode);

        switch (policyLineDefinition)
        {
            case null:
                throw new Exception($"PLD {request.PolicyLineDefinitionCode} does not exist");
            default:
                response.PolicyLineDefinitionName = policyLineDefinition.Name;
                response.Questions = policyLineDefinition.Questions.Select(
                    x => new QuestionForRiskDataCapture
                    {
                        Code = x.Code,
                        Label = x.Label,
                        Type = (QuestionTypeForRiskDataCapture) x.Type,
                        Options = x.Options?.Select(y => new DropDownOptionForRiskDataCapture { Name = y.Name, Value = y.Value }).ToArray()
                    }).ToArray();
                break;
        }
    }

    public Task Rollback(Exception exception) => Task.CompletedTask;
}