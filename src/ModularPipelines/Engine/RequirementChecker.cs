using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;

namespace ModularPipelines.Engine;

internal class RequirementChecker : IRequirementChecker
{
    private readonly IPipelineContextProvider _moduleContextProvider;
    private readonly List<IPipelineRequirement> _requirements;

    public RequirementChecker(IEnumerable<IPipelineRequirement> requirements, IPipelineContextProvider moduleContextProvider)
    {
        _moduleContextProvider = moduleContextProvider;
        _requirements = requirements.ToList();
    }

    public async Task CheckRequirementsAsync()
    {
        var failedRequirementsNames = new List<string>();

        await _requirements.ToAsyncProcessorBuilder()
            .ForEachAsync(async requirement =>
        {
            var requirementDecision = await requirement.MustAsync(await _moduleContextProvider.GetModuleContext());

            if (!requirementDecision.Success)
            {
                failedRequirementsNames.Add(requirementDecision.Reason ?? requirement.GetType().Name);
            }
        }).ProcessInParallel();

        if (failedRequirementsNames.Any())
        {
            throw new FailedRequirementsException($"Requirements failed:\r\n{string.Join("\r\n", failedRequirementsNames)}");
        }
    }
}
