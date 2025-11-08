using System.Collections.Concurrent;
using Mediator;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Enums;
using ModularPipelines.Events;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Service for executing sub-modules with progress tracking.
/// Replaces the SubModule methods previously embedded in ModuleBase.
/// </summary>
public class ModuleSubModuleService : IModuleSubModuleService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Guid, List<SubModuleBase>> _subModules = new();
    private readonly ConcurrentDictionary<(Guid ModuleId, string Name), SubModuleBase> _subModuleCache = new();

    public ModuleSubModuleService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResult> ExecuteSubModuleAsync<TResult>(IModule parentModule, string name, Func<Task<TResult>> action)
    {
        var cacheKey = (parentModule.Id, name);

        // Check for existing sub-module with same name
        if (_subModuleCache.TryGetValue(cacheKey, out var existingSubModule))
        {
            if (existingSubModule.Status == Status.Successful && existingSubModule is SubModule<TResult> typedSubModule)
            {
                // Return cached result
                return await typedSubModule.SubModuleResultTaskCompletionSource.Task;
            }
            else if (existingSubModule.Status is Status.NotYetStarted or Status.Processing)
            {
                throw new Exception("Use Distinct names for SubModules");
            }
        }

        // Create new sub-module
        var subModule = new SubModule<TResult>(parentModule.ModuleType, name);

        // Register in cache and collection
        _subModuleCache[cacheKey] = subModule;
        _subModules.AddOrUpdate(
            parentModule.Id,
            _ => new List<SubModuleBase> { subModule },
            (_, list) => { list.Add(subModule); return list; });

        // Publish creation event
        var mediator = _serviceProvider.GetService<IMediator>();
        if (mediator != null)
        {
            var estimatedDuration = TimeSpan.FromMinutes(2); // Default estimation
            await mediator.Publish(new SubModuleCreatedNotification(
                parentModule as ModuleBase ?? throw new InvalidOperationException("Parent must be ModuleBase for sub-module support"),
                subModule,
                estimatedDuration));
        }

        // Execute sub-module
        var result = await subModule.Execute(action);

        // Publish completion event
        if (mediator != null)
        {
            var isSuccessful = subModule.Status == Status.Successful;
            await mediator.Publish(new SubModuleCompletedNotification(
                parentModule as ModuleBase ?? throw new InvalidOperationException("Parent must be ModuleBase for sub-module support"),
                subModule,
                isSuccessful));
        }

        return result;
    }
}
