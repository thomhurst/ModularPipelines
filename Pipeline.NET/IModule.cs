using System.Runtime.CompilerServices;

namespace Pipeline.NET;

public interface IModule
{
    internal Task OnBeforeExecute();
    
    internal Task InitialiseAsync();
    
    internal Task<ModuleResult> StartProcessingModule();
    
    Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken);

    internal Task OnAfterExecute();
    
    DateTimeOffset StartTime { get; }
    DateTimeOffset EndTime { get; }
    
    Status Status { get; }
    
    internal TimeSpan Timeout { get; }
    
    TimeSpan Duration { get; }

    protected Task<TModule> GetModuleAsync<TModule>() where TModule : IModule;
    
    protected Task WaitForModule<TModule>() where TModule : IModule;
    
    TaskAwaiter<ModuleResult> GetAwaiter();
}