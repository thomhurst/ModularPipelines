using ModularPipelines.Reporting;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Testing;

internal sealed class NoOpConsoleServices :
    IConsoleCoordinator,
    IOutputCoordinator,
    IProgressDisplay,
    IProgressSession
{
    public static NoOpConsoleServices Instance { get; } = new();

    private static NoOpModuleOutputBuffer Buffer { get; } = new();

    public void Install()
    {
    }

    public Task<IProgressSession> BeginProgressAsync(
        OrganizedModules modules,
        CancellationToken cancellationToken) =>
        Task.FromResult<IProgressSession>(this);

    public IModuleOutputBuffer GetModuleBuffer(Type moduleType) => Buffer;

    public ModuleOutputExcerpt? GetModuleOutputExcerpt(Type moduleType) => null;

    public IModuleOutputBuffer GetUnattributedBuffer() => Buffer;

    public Task<IReadOnlyList<IModuleOutputBuffer>> FlushPendingWritesAsync() =>
        Task.FromResult<IReadOnlyList<IModuleOutputBuffer>>([]);

    public Task FlushInProgressModuleOutputAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task FlushModuleOutputAsync() => Task.CompletedTask;

    public void WriteResults(PipelineSummary summary)
    {
    }

    public void AddDeferredException(string message)
    {
    }

    public void WriteExceptions()
    {
    }

    public void Uninstall()
    {
    }

    public void EnableOutputBuffering()
    {
    }

    public void SetProgressController(IProgressController controller)
    {
    }

    public Task EnqueueAndFlushAsync(
        IModuleOutputBuffer buffer,
        OutputFlushKind flushKind,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task WaitForPendingFlushesAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public void SetProgressActive(bool isActive)
    {
    }

    public Task OnModuleCompletedAsync(
        IModuleOutputBuffer buffer,
        Type moduleType,
        CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task FlushDeferredAsync(CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task RunAsync(
        OrganizedModules organizedModules,
        CancellationToken cancellationToken) =>
        Task.CompletedTask;

    public void OnModuleStarted(ModuleState moduleState, TimeSpan estimatedDuration)
    {
    }

    public void OnModuleCompleted(ModuleState moduleState, bool isSuccessful)
    {
    }

    public void OnModuleSkipped(ModuleState moduleState)
    {
    }

    public void OnSubModuleCreated(
        IModule parentModule,
        SubModuleBase subModule,
        TimeSpan estimatedDuration)
    {
    }

    public void OnSubModuleCompleted(SubModuleBase subModule, bool isSuccessful)
    {
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;

    private sealed class NoOpModuleOutputBuffer : IModuleOutputBuffer
    {
        public Type ModuleType => typeof(void);

        public bool HasOutput => false;

        public bool IsComplete => true;

        public bool NeedsCompletionFlush => false;

        public void WriteLine(string message)
        {
        }

        public void WriteGroupCommand(IBuildSystemFormatter formatter, string? command)
        {
        }

        public void AddLogEvent(IBufferedLogEvent logEvent)
        {
        }

        public void SetException(Exception exception)
        {
        }

        public void MarkComplete()
        {
        }

        public Task FlushToAsync(
            TextWriter console,
            IBuildSystemFormatter formatter,
            ILogger logger,
            ISpectreConsoleLoggerControl loggerControl,
            OutputFlushKind flushKind,
            IReadOnlyList<ILogger>? fallbackLoggers = null,
            CancellationToken cancellationToken = default) =>
            Task.CompletedTask;
    }
}
