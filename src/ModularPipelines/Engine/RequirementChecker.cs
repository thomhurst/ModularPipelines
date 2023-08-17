using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.Engine;

internal class RequirementChecker : IRequirementChecker
{
    private readonly IModuleContextProvider _moduleContextProvider;
    private readonly List<IPipelineRequirement> _requirements;

    public RequirementChecker(IEnumerable<IPipelineRequirement> requirements, IModuleContextProvider moduleContextProvider)
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
            var mustAsync = await requirement.MustAsync(_moduleContextProvider.GetModuleContext());
            
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
