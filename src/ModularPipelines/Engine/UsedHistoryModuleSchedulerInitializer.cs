using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

internal static class UsedHistoryModuleSchedulerInitializer
{
    public static void Precomplete(
        IReadOnlyList<IModule> modules,
        IModuleScheduler scheduler,
        IModuleResultRegistry resultRegistry)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();
            var existingResult = resultRegistry.GetResult(moduleType);
            if (existingResult?.ModuleStatus != Status.UsedHistory)
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
                statusOverride: Status.UsedHistory);
        }
    }
}
