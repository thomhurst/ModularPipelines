using System.ComponentModel;
using System.Runtime.CompilerServices;
using ModularPipelines.Enums;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

public interface IModule
{
    internal Task Task { get; }

    DateTimeOffset StartTime { get; }
    DateTimeOffset EndTime { get; }
    
    Status Status { get; internal set; }
    
    internal TimeSpan Timeout { get; }
    
    TimeSpan Duration { get; }
    
    bool ShouldSkip { get; }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal Task StartProcessingModule();
}

public interface IModule<T> : IModule
{
    TaskAwaiter<ModuleResult<T>> GetAwaiter();
}