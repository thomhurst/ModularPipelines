using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Console;

/// <summary>
/// Progress controller for CI environments where no dynamic display exists.
/// All operations are no-ops.
/// </summary>
[ExcludeFromCodeCoverage]
internal sealed class NoOpProgressController : IProgressController
{
    public static NoOpProgressController Instance { get; } = new();

    public bool IsInteractive => false;

    public Task PauseAsync() => Task.CompletedTask;

    public Task ResumeAsync() => Task.CompletedTask;
}
