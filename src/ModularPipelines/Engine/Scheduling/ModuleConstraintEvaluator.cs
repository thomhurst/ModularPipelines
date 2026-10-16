using Microsoft.Extensions.Logging;
using ModularPipelines.Helpers;

namespace ModularPipelines.Engine.Scheduling;

/// <summary>
/// Evaluates module execution constraints (NotInParallel, lock keys, and parallel limits).
/// </summary>
internal class ModuleConstraintEvaluator(ILogger<ModuleConstraintEvaluator> logger) : IModuleConstraintEvaluator
{
    private readonly ILogger<ModuleConstraintEvaluator> _logger = logger;

    /// <inheritdoc />
    public bool CanQueue(ModuleState moduleState, IEnumerable<ModuleState> activeModules)
    {
        var activeCollection = AsReadOnlyCollection(activeModules);

        // Check lock key conflicts
        if (!CheckLockKeyConstraints(moduleState, activeCollection, logBlocking: false))
        {
            return false;
        }

        // Check sequential execution constraints
        if (!CheckSequentialConstraints(moduleState, activeCollection, logBlocking: false))
        {
            return false;
        }

        // Queued modules reserve capacity too, so limiter waiters never fill the worker pool.
        return CheckParallelLimits(moduleState, activeCollection);
    }

    /// <inheritdoc />
    public bool CanStartExecution(ModuleState moduleState, IEnumerable<ModuleState> executingModules)
    {
        var executingCollection = AsReadOnlyCollection(executingModules);

        // Primary constraint check: verify no executing module has conflicting lock keys
        if (!CheckLockKeyConstraints(moduleState, executingCollection, logBlocking: true))
        {
            return false;
        }

        // Secondary check: sequential execution constraints
        if (!CheckSequentialConstraints(moduleState, executingCollection, logBlocking: true))
        {
            return false;
        }

        return true;
    }

    private static IReadOnlyCollection<ModuleState> AsReadOnlyCollection(IEnumerable<ModuleState> modules)
    {
        return modules as IReadOnlyCollection<ModuleState> ?? [.. modules];
    }

    private static bool CheckParallelLimits(ModuleState moduleState, IReadOnlyCollection<ModuleState> activeModules)
    {
        var limiter = moduleState.ParallelLimiter;
        var hintLimit = moduleState.ExecutionHintLimit;
        if (limiter is null && !hintLimit.HasValue)
        {
            return true;
        }

        var limiterCount = 0;
        var hintCount = 0;
        foreach (var active in activeModules)
        {
            if (active == moduleState)
            {
                continue;
            }

            if (limiter is not null && Equals(active.ParallelLimiter?.Key, limiter.Key)
                && ++limiterCount >= limiter.Limit)
            {
                return false;
            }

            if (hintLimit.HasValue && active.ExecutionHint == moduleState.ExecutionHint
                && ++hintCount >= hintLimit.Value)
            {
                return false;
            }
        }

        return true;
    }

    private bool CheckLockKeyConstraints(
        ModuleState moduleState,
        IReadOnlyCollection<ModuleState> otherModules,
        bool logBlocking)
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

            if (HasLockConflict(other.RequiredLockKeys, moduleState.RequiredLockKeys))
            {
                if (logBlocking)
                {
                    var conflictingKeys = other.RequiredLockKeys
                        .Where(key => Array.IndexOf(moduleState.RequiredLockKeys, key) >= 0)
                        .ToArray();
                    _logger.LogDebug(
                        "Module {ModuleName} BLOCKED - {OtherModule} has conflicting keys [{Keys}]",
                        moduleState.ModuleType.Name,
                        other.ModuleType.Name,
                        string.Join(", ", conflictingKeys));
                }

                return false;
            }
        }

        return true;
    }

    private static bool HasLockConflict(string[] left, string[] right)
    {
        foreach (var key in left)
        {
            if (Array.IndexOf(right, key) >= 0)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckSequentialConstraints(
        ModuleState moduleState,
        IReadOnlyCollection<ModuleState> otherModules,
        bool logBlocking)
    {
        var otherActiveModuleCount = 0;

        foreach (var other in otherModules)
        {
            if (other == moduleState)
            {
                continue;
            }

            otherActiveModuleCount++;

            if (moduleState.RequiresSequentialExecution)
            {
                if (!logBlocking)
                {
                    return false;
                }

                continue;
            }

            if (other.RequiresSequentialExecution)
            {
                if (logBlocking)
                {
                    _logger.LogDebug(
                        "Module {ModuleName} blocked by sequential module {SequentialModule}",
                        moduleState.ModuleType.Name,
                        other.ModuleType.Name);
                }

                return false;
            }
        }

        if (moduleState.RequiresSequentialExecution && otherActiveModuleCount > 0)
        {
            _logger.LogDebug(
                "Sequential module {ModuleName} blocked - {Count} modules already active",
                moduleState.ModuleType.Name,
                otherActiveModuleCount);

            return false;
        }

        return true;
    }
}
