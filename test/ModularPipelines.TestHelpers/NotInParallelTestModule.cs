using System.Collections.Concurrent;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.TestHelpers;

/// <summary>
/// Base class for modules used in NotInParallel tests.
/// Tracks execution timing to detect parallel execution violations.
/// </summary>
public abstract class NotInParallelTestModule : Module<string>
{
    /// <summary>
    /// Gets the tracker instance for this test. Override to provide a shared tracker.
    /// </summary>
    protected abstract NotInParallelTracker Tracker { get; }

    /// <summary>
    /// Gets the names of modules that should NOT be executing at the same time as this module.
    /// </summary>
    protected abstract IEnumerable<string> ConflictingModuleNames { get; }

    /// <summary>
    /// Delay in milliseconds for the execution. Default is 50ms (50ms before check, 50ms after).
    /// </summary>
    protected virtual int DelayMs => 50;

    protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var moduleName = GetType().Name;

        Tracker.MarkExecuting(moduleName);
        await Task.Delay(DelayMs, cancellationToken);

        foreach (var conflicting in ConflictingModuleNames)
        {
            if (Tracker.IsExecuting(conflicting))
            {
                Tracker.AddViolation($"{moduleName} started while {conflicting} was executing");
            }
        }

        await Task.Delay(DelayMs, cancellationToken);

        Tracker.MarkCompleted(moduleName);
        return moduleName;
    }
}

/// <summary>
/// Tracks module execution for NotInParallel tests.
/// Uses ConcurrentDictionary to ensure correct tracking (ConcurrentBag.TryTake removes arbitrary items).
/// </summary>
public class NotInParallelTracker
{
    private readonly ConcurrentDictionary<string, bool> _executingModules = new();
    private readonly ConcurrentBag<string> _violations = new();

    /// <summary>
    /// Marks a module as currently executing.
    /// </summary>
    public void MarkExecuting(string moduleName) => _executingModules[moduleName] = true;

    /// <summary>
    /// Marks a module as completed (no longer executing).
    /// </summary>
    public void MarkCompleted(string moduleName) => _executingModules.TryRemove(moduleName, out _);

    /// <summary>
    /// Checks if a module is currently executing.
    /// </summary>
    public bool IsExecuting(string moduleName) => _executingModules.ContainsKey(moduleName);

    /// <summary>
    /// Adds a violation message.
    /// </summary>
    public void AddViolation(string message) => _violations.Add(message);

    /// <summary>
    /// Gets all recorded violations.
    /// </summary>
    public IReadOnlyCollection<string> Violations => _violations.ToArray();

    /// <summary>
    /// Resets the tracker state. Call at the beginning of each test.
    /// </summary>
    public void Reset()
    {
        _executingModules.Clear();
        while (_violations.TryTake(out _)) { }
    }
}
