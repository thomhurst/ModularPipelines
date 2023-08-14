using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;

namespace ModularPipelines.Engine;

internal class RequirementChecker : IRequirementChecker
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly List<IPipelineRequirement> _requirements;

    public RequirementChecker(IEnumerable<IPipelineRequirement> requirements, IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _requirements = requirements.ToList();
    }

    public async Task CheckRequirementsAsync()
    {
        var failedRequirementsNames = new List<string>();
        
        await _requirements.ToAsyncProcessorBuilder()
            .ForEachAsync(async requirement =>
        {
            var serviceScope = _serviceScopeFactory.CreateScope();

            var mustAsync = await requirement.MustAsync(serviceScope.ServiceProvider.GetRequiredService<IModuleContext>());
            
            serviceScope.Dispose();

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
