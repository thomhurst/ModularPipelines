using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Scheduling;

/// <summary>
/// Evaluates module execution constraints (NotInParallel, lock keys).
/// </summary>
internal class ModuleConstraintEvaluator : IModuleConstraintEvaluator
{
    private readonly ILogger<ModuleConstraintEvaluator> _logger;

    public ModuleConstraintEvaluator(ILogger<ModuleConstraintEvaluator> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public bool CanQueue(ModuleState moduleState, IEnumerable<ModuleState> activeModules)
    {
        var activeList = activeModules.ToList();

        // Check lock key conflicts
        if (!CheckLockKeyConstraints(moduleState, activeList, logBlocking: false))
        {
            return false;
        }

        // Check sequential execution constraints
        if (!CheckSequentialConstraints(moduleState, activeList, logBlocking: false))
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc />
    public bool CanStartExecution(ModuleState moduleState, IEnumerable<ModuleState> executingModules)
    {
        var executingList = executingModules.ToList();

        // Primary constraint check: verify no executing module has conflicting lock keys
        if (!CheckLockKeyConstraints(moduleState, executingList, logBlocking: true))
        {
            return false;
        }

        // Secondary check: sequential execution constraints
        if (!CheckSequentialConstraints(moduleState, executingList, logBlocking: true))
        {
            return false;
        }

        return true;
    }

    private bool CheckLockKeyConstraints(ModuleState moduleState, List<ModuleState> otherModules, bool logBlocking)
    {
        if (moduleState.RequiredLockKeys.Length == 0)
        {
            return true;
        }

        foreach (var other in otherModules)
        {
            if (other == moduleState)
            {
                continue;
            }

            if (other.RequiredLockKeys.Length == 0)
            {
                continue;
            }

            var intersection = other.RequiredLockKeys.Intersect(moduleState.RequiredLockKeys).ToArray();
            if (intersection.Length > 0)
            {
                if (logBlocking)
                {
                    _logger.LogDebug(
                        "Module {ModuleName} BLOCKED - {OtherModule} has conflicting keys [{Keys}]",
                        MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
                        MarkupFormatter.FormatModuleName(other.ModuleType.Name),
                        string.Join(", ", intersection));
                }

                return false;
            }
        }

        return true;
    }

    private bool CheckSequentialConstraints(ModuleState moduleState, List<ModuleState> otherModules, bool logBlocking)
    {
        var otherActiveModules = otherModules.Where(m => m != moduleState).ToList();

        // If this module requires sequential execution, no other modules can be active
        if (moduleState.RequiresSequentialExecution && otherActiveModules.Count > 0)
        {
            if (logBlocking)
            {
                _logger.LogDebug(
                    "Sequential module {ModuleName} blocked - {Count} modules already active",
                    MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
                    otherActiveModules.Count);
            }

            return false;
        }

        // If any other active module requires sequential execution, this module is blocked
        foreach (var other in otherActiveModules)
        {
            if (other.RequiresSequentialExecution)
            {
                if (logBlocking)
                {
                    _logger.LogDebug(
                        "Module {ModuleName} blocked by sequential module {SequentialModule}",
                        MarkupFormatter.FormatModuleName(moduleState.ModuleType.Name),
                        MarkupFormatter.FormatModuleName(other.ModuleType.Name));
                }

                return false;
            }
        }

        return true;
    }
}
