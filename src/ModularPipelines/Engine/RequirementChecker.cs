using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Requirements;
using TomLonghurst.EnumerableAsyncProcessor.Extensions;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization.Extensions;

namespace ModularPipelines.Engine;

internal class RequirementChecker : IRequirementChecker
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IPipelineRequirement> _requirements;

    public RequirementChecker(IEnumerable<IPipelineRequirement> requirements, IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _requirements = requirements.ToList();
    }

    public async Task CheckRequirementsAsync()
    {
        var failedRequirementsNames = new List<string>();
        
        await _requirements.ToAsyncProcessorBuilder()
            .ForEachAsync(async requirement =>
        {
            await using var serviceScope = _serviceProvider.CreateAsyncScope();
            await serviceScope.ServiceProvider.InitializeAsync();
            
            var mustAsync = await requirement.MustAsync(_serviceProvider.GetRequiredService<IModuleContext>());
            
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
