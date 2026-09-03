using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class ConsoleCaptureContractTests
{
    private sealed class ConcurrentStartGate
    {
        private readonly TaskCompletionSource _bothStarted =
            new(TaskCreationOptions.RunContinuationsAsynchronously);
        private int _started;

        public Task SignalAndWaitAsync()
        {
            if (Interlocked.Increment(ref _started) == 2)
            {
                _bothStarted.SetResult();
            }

            return _bothStarted.Task;
        }
    }

    private sealed class FirstConsoleModule(ConcurrentStartGate gate) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            System.Console.Write("first-out-");
            await gate.SignalAndWaitAsync().ConfigureAwait(false);
            await Task.Yield();
            System.Console.WriteLine("continued");
            await Task.Run(static () =>
            {
                System.Console.Error.Write("first-error-");
                System.Console.Error.WriteLine("task-run");
            }, cancellationToken).ConfigureAwait(false);
            context.Console.WriteLine("first-rich");
            context.Logger.LogWarning("first-log");
            return true;
        }
    }

    private sealed class SecondConsoleModule(ConcurrentStartGate gate) : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            System.Console.Write("second-out-");
            await gate.SignalAndWaitAsync().ConfigureAwait(false);
            await Task.Yield();
            System.Console.WriteLine("continued");
            await Task.Run(static () =>
            {
                System.Console.Error.Write("second-error-");
                System.Console.Error.WriteLine("task-run");
            }, cancellationToken).ConfigureAwait(false);
            context.Console.WriteLine("second-rich");
            context.Logger.LogWarning("second-log");
            return true;
        }
    }

    private sealed class ConstructorAndSubModuleConsoleModule : Module<bool>
    {
        public ConstructorAndSubModuleConsoleModule()
        {
            System.Console.WriteLine("constructor-output");
        }

        protected internal override async Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await context.RunSubModuleAsync(
                    "console-child",
                    _ =>
                    {
                        System.Console.WriteLine("submodule-output");
                        return Task.CompletedTask;
                    })
                .ConfigureAwait(false);
            return true;
        }
    }

    [Test]
    public async Task ConcurrentModulesKeepDirectConsoleOutputInTheirOwnOrderedStreams()
    {
        var builder = CreateReportingBuilder();
        builder.Services.AddSingleton<ConcurrentStartGate>();
        builder.AddModule<FirstConsoleModule>();
        builder.AddModule<SecondConsoleModule>();

        var summary = await builder.RunAsync();
        var first = summary.RunReport!.Modules.Single(module =>
            module.ModuleName == nameof(FirstConsoleModule));
        var second = summary.RunReport.Modules.Single(module =>
            module.ModuleName == nameof(SecondConsoleModule));

        using (Assert.Multiple())
        {
            await AssertModuleOutput(
                first.Output,
                "first-out-continued",
                "first-rich",
                "first-log",
                "first-error-task-run",
                "second-");
            await AssertModuleOutput(
                second.Output,
                "second-out-continued",
                "second-rich",
                "second-log",
                "second-error-task-run",
                "first-");
        }
    }

    [Test]
    public async Task ConstructionAndSubModuleWritesUseTheOwningModuleBuffer()
    {
        var summary = await CreateReportingBuilder()
            .AddModule<ConstructorAndSubModuleConsoleModule>()
            .RunAsync();
        var output = summary.RunReport!.Modules.Single().Output!.StdoutTail!;

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("constructor-output");
            await Assert.That(output).Contains("submodule-output");
        }
    }

    [Test]
    public async Task LateFlowedWriteIsReassignedToUnattributedOutput()
    {
        var moduleBuffer = new Mock<IModuleOutputBuffer>();
        var unattributedBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetModuleBuffer(typeof(FirstConsoleModule)))
            .Returns(moduleBuffer.Object);
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(unattributedBuffer.Object);
        using var writer = CreateWriter(coordinator.Object);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task lateWrite;

        using (new ModuleOutputContextScope(typeof(FirstConsoleModule)))
        {
            lateWrite = Task.Run(async () =>
            {
                await release.Task.ConfigureAwait(false);
                writer.WriteLine("late-output");
            });
        }

        release.SetResult();
        await lateWrite;

        unattributedBuffer.Verify(x => x.WriteLine("late-output"), Times.Once);
        moduleBuffer.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public void WriteWithoutModuleContextUsesUnattributedOutput()
    {
        var unattributedBuffer = new Mock<IModuleOutputBuffer>();
        var coordinator = new Mock<IConsoleCoordinator>();
        coordinator.Setup(x => x.GetUnattributedBuffer()).Returns(unattributedBuffer.Object);
        using var writer = CreateWriter(coordinator.Object);

        writer.WriteLine("pipeline-output");

        unattributedBuffer.Verify(x => x.WriteLine("pipeline-output"), Times.Once);
    }

    private static PipelineBuilder CreateReportingBuilder()
    {
        var builder = TestPipelineBuilder.Create();
        builder.ConfigureOptions(options => options with
        {
            Console = options.Console with { ShowProgress = false },
            RunReport = options.RunReport with
            {
                IncludeModuleOutput = true,
                MaxOutputBytesPerModule = 4096,
            },
        });
        return builder;
    }

    private static CoordinatedTextWriter CreateWriter(IConsoleCoordinator coordinator)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Version).Returns(0);
        secretProvider.Setup(provider => provider.GetSnapshot()).Returns(new SecretSnapshot(0, []));
        var obfuscator = new Mock<ISecretObfuscator>();
        obfuscator.Setup(x => x.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        return new CoordinatedTextWriter(
            coordinator,
            new StringWriter(),
            static () => true,
            obfuscator.Object,
            secretProvider.Object);
    }

    private static async Task AssertModuleOutput(
        Reporting.ModuleOutputExcerpt? output,
        string stdout,
        string rich,
        string log,
        string stderr,
        string foreignPrefix)
    {
        await Assert.That(output).IsNotNull();
        var stdoutTail = output!.StdoutTail!;
        var stderrTail = output.StderrTail!;

        using (Assert.Multiple())
        {
            await Assert.That(Count(stdoutTail, stdout)).IsEqualTo(1);
            await Assert.That(Count(stdoutTail, rich)).IsEqualTo(1);
            await Assert.That(Count(stdoutTail, log)).IsEqualTo(1);
            await Assert.That(stdoutTail.IndexOf(stdout, StringComparison.Ordinal))
                .IsLessThan(stdoutTail.IndexOf(rich, StringComparison.Ordinal));
            await Assert.That(stdoutTail.IndexOf(rich, StringComparison.Ordinal))
                .IsLessThan(stdoutTail.IndexOf(log, StringComparison.Ordinal));
            await Assert.That(Count(stderrTail, stderr)).IsEqualTo(1);
            await Assert.That(stdoutTail).DoesNotContain(stderr);
            await Assert.That(stderrTail).DoesNotContain(stdout);
            await Assert.That(stdoutTail).DoesNotContain(foreignPrefix);
            await Assert.That(stderrTail).DoesNotContain(foreignPrefix);
        }
    }

    private static int Count(string value, string substring) =>
        value.Split(substring, StringSplitOptions.None).Length - 1;
}
