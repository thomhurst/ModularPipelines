using System.Collections.Concurrent;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Services;

/// <summary>
/// Thread-safe service for tracking module state across the pipeline.
/// Replaces the state management previously embedded in ModuleBase.
/// </summary>
public class ModuleStateTracker : IModuleStateTracker
{
    private readonly ConcurrentDictionary<Guid, ModuleState> _moduleStates = new();

    public Status GetStatus(IModule module)
    {
        return GetState(module).Status;
    }

    public void SetStatus(IModule module, Status status)
    {
        GetState(module).Status = status;
    }

    public DateTimeOffset? GetStartTime(IModule module)
    {
        return GetState(module).StartTime;
    }

    public void SetStartTime(IModule module, DateTimeOffset startTime)
    {
        GetState(module).StartTime = startTime;
    }

    public DateTimeOffset? GetEndTime(IModule module)
    {
        return GetState(module).EndTime;
    }

    public void SetEndTime(IModule module, DateTimeOffset endTime)
    {
        GetState(module).EndTime = endTime;
    }

    public TimeSpan? GetDuration(IModule module)
    {
        var state = GetState(module);
        if (state.StartTime.HasValue && state.EndTime.HasValue)
        {
            return state.EndTime.Value - state.StartTime.Value;
        }
        return null;
    }

    public Exception? GetException(IModule module)
    {
        return GetState(module).Exception;
    }

    public void SetException(IModule module, Exception exception)
    {
        GetState(module).Exception = exception;
    }

    private ModuleState GetState(IModule module)
    {
        return _moduleStates.GetOrAdd(module.Id, _ => new ModuleState());
    }

    private class ModuleState
    {
        public Status Status { get; set; } = Status.NotYetStarted;
        public DateTimeOffset? StartTime { get; set; }
        public DateTimeOffset? EndTime { get; set; }
        public Exception? Exception { get; set; }
    }
}
