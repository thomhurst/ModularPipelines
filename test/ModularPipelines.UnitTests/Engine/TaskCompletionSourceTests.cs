using System.Reflection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class TaskCompletionSourceTests
{
    [Test]
    public async Task ModuleState_CompletionSource_RunsContinuationsAsynchronously()
    {
        var state = new ModuleState(Mock.Of<IModule>(), typeof(IModule));

        await Assert.That(RunsContinuationsAsynchronously(state.CompletionSource.Task)).IsTrue();
    }

    [Test]
    public async Task ModuleResultRegistry_CompletionSource_RunsContinuationsAsynchronously()
    {
        var registry = new ModuleResultRegistry();
        registry.RegisterModule(typeof(IModule));

        await Assert.That(RunsContinuationsAsynchronously(registry.GetCompletionTask(typeof(IModule))!)).IsTrue();
    }

    [Test]
    public async Task ProgressSession_CompletionSource_RunsContinuationsAsynchronously()
    {
        var session = new ProgressSession(
            null!,
            new OrganizedModules([], []),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            CancellationToken.None);
        var field = typeof(ProgressSession).GetField("_progressCompleted", BindingFlags.Instance | BindingFlags.NonPublic);
        var completionSource = (TaskCompletionSource) field!.GetValue(session)!;

        await Assert.That(RunsContinuationsAsynchronously(completionSource.Task)).IsTrue();
    }

    private static bool RunsContinuationsAsynchronously(Task task) =>
        task.CreationOptions.HasFlag(TaskCreationOptions.RunContinuationsAsynchronously);
}
