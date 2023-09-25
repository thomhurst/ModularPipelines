using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

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
            var mustAsync = await requirement.MustAsync(await _moduleContextProvider.GetModuleContext());

            if (!mustAsync)
            {
                failedRequirementsNames.Add(requirement.GetType().Name);
            }
        }).ProcessInParallel();

        if (failedRequirementsNames.Any())
        {
            throw new FailedRequirementsException($"Requirements failed: {string.Join(" | ", failedRequirementsNames)}");
        }
    }
}
