using System.Collections.Concurrent;
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

    public async Task CheckRequirementsAsync(CancellationToken cancellationToken)
    {
        var failedRequirementsNames = new ConcurrentBag<string>();

        var groupedRequirements = _requirements
            .GroupBy(x => x.Order)
            .OrderBy(group => group.Key);

        foreach (var pipelineRequirements in groupedRequirements)
        {
            await pipelineRequirements.ToAsyncProcessorBuilder()
                .ForEachAsync(async requirement =>
                {
                    var requirementDecision = await requirement
                        .EvaluateAsync(_moduleContextProvider.GetModuleContext(), cancellationToken)
                        .ConfigureAwait(false);

                    if (!requirementDecision.IsSatisfied)
                    {
                        failedRequirementsNames.Add(requirementDecision.Reason ?? requirement.GetType().Name);
                    }
                }).ProcessInParallel();
        }

        if (!failedRequirementsNames.IsEmpty)
        {
            throw new RequirementNotMetException($"Requirements failed:\r\n{string.Join("\r\n", failedRequirementsNames)}");
        }
    }
}
