using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

namespace ModularPipelines.Engine.Constraints;

/// <summary>
/// Enforces lock key constraints (NotInParallel attribute with specific keys).
/// </summary>
internal class LockKeyConstraint : IModuleConstraint
{
    public bool CanExecute(ModuleState moduleState, ModuleStateQueries stateQueries)
    {
        if (moduleState.RequiredLockKeys.Length == 0)
        {
            return true;
        }

        return stateQueries.FindModuleWithLockConflict(moduleState) == null;
    }

    public void LogViolation(ModuleState moduleState, ModuleStateQueries stateQueries, ILogger logger)
    {
        var conflictingModule = stateQueries.FindModuleWithLockConflict(moduleState);
        if (conflictingModule == null)
        {
            return;
        }

        var conflictingKeys = string.Join(", ",
            moduleState.RequiredLockKeys.Intersect(conflictingModule.RequiredLockKeys));

        logger.LogDebug(
            "Module {ModuleName} blocked by lock conflict with {ConflictingModule} on keys: {Keys}",
            MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
            MarkupFormatter.FormatModuleName(conflictingModule.Module.GetType().Name),
            conflictingKeys);
    }
}
