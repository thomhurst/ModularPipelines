using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

namespace ModularPipelines.Engine.Constraints;

/// <summary>
/// Enforces sequential execution constraints (NotInParallel attribute without keys).
/// </summary>
internal class SequentialExecutionConstraint : IModuleConstraint
{
    public bool CanExecute(ModuleState moduleState, ModuleStateQueries stateQueries)
    {
        // Check if blocked by another sequential module
        var blockingModule = stateQueries.FindBlockingSequentialModule(moduleState);
        if (blockingModule != null)
        {
            return false;
        }

        // Check if this is a sequential module blocked by others
        if (moduleState.RequiresSequentialExecution)
        {
            return !stateQueries.AreOtherModulesActive(moduleState);
        }

        return true;
    }

    public void LogViolation(ModuleState moduleState, ModuleStateQueries stateQueries, ILogger logger)
    {
        var blockingModule = stateQueries.FindBlockingSequentialModule(moduleState);
        if (blockingModule != null)
        {
            logger.LogDebug(
                "Module {ModuleName} blocked by sequential module {SequentialModule}",
                MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name),
                MarkupFormatter.FormatModuleName(blockingModule.Module.GetType().Name));
            return;
        }

        if (moduleState.RequiresSequentialExecution)
        {
            logger.LogDebug(
                "Sequential module {ModuleName} blocked - other modules still running/queued",
                MarkupFormatter.FormatModuleName(moduleState.Module.GetType().Name));
        }
    }
}
