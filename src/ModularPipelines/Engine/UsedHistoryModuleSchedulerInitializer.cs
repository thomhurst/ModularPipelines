using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal static class UsedHistoryModuleSchedulerInitializer
{
    public static HashSet<Type> GetPrecompletedModuleTypes(
        IReadOnlyList<IModule> modules,
        IModuleResultRegistry resultRegistry)
    {
        return modules
            .Select(module => module.GetType())
            .Where(moduleType => resultRegistry.GetResult(moduleType)?.Status == ModuleStatus.RestoredFromHistory)
            .ToHashSet();
    }

    public static void Precomplete(
        IReadOnlyList<IModule> modules,
        IModuleScheduler scheduler,
        IModuleResultRegistry resultRegistry)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var existingResult = resultRegistry.GetResult(moduleType);
            if (existingResult?.Status != ModuleStatus.RestoredFromHistory)
            {
                continue;
            }

            var moduleState = scheduler.GetModuleState(moduleType);
            if (moduleState is not null)
            {
                moduleState.Result = existingResult;
            }

            scheduler.MarkModuleCompleted(
                moduleType,
                success: true,
                statusOverride: ModuleStatus.RestoredFromHistory);
        }
    }
}
