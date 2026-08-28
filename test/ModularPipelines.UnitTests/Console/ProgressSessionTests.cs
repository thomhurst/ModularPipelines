using ModularPipelines.Secrets;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Console;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;
using Spectre.Console;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class ProgressSessionTests
{
    [Test]
    public async Task StartAsync_Renders_Pipeline_And_Module_Rows()
    {
        using var output = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Yes,
            ColorSystem = ColorSystemSupport.Standard,
            Interactive = InteractionSupport.Yes,
            Out = new AnsiConsoleOutput(output),
        });
        var outputCoordinator = new Mock<IOutputCoordinator>();
        var coordinator = CreateCoordinator(outputCoordinator.Object, console);
        var module = new RenderingModule();
        var organizedModules = new OrganizedModules(
            [new RunnableModule(module, TimeSpan.Zero)],
            []);
        await using var session = new ProgressSession(
            coordinator,
            organizedModules,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            console,
            CancellationToken.None);

        await session.StartAsync();
        var moduleState = new ModuleState(module, module.GetType());
        session.OnModuleStarted(moduleState, TimeSpan.Zero);
        session.OnModuleCompleted(moduleState, true);
        await session.DisposeAsync();

        var renderedProgress = output.ToString();
        await Assert.That(renderedProgress).Contains("Pipeline");
        await Assert.That(renderedProgress).Contains("Rendering");
    }

    [Test]
    public async Task PauseAsync_CompletesImmediatelyWhenNoRefreshIsActive()
    {
        var outputCoordinator = new Mock<IOutputCoordinator>();
        var coordinator = CreateCoordinator(outputCoordinator.Object);
        await using var session = new ProgressSession(
            coordinator,
            new OrganizedModules([], []),
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            DelegatingAnsiConsole.Instance,
            CancellationToken.None);

        var pauseTask = session.PauseAsync();

        await Assert.That(pauseTask.IsCompleted).IsTrue();
        await session.ResumeAsync();
    }

    private static ConsoleCoordinator CreateCoordinator(
        IOutputCoordinator outputCoordinator,
        IAnsiConsole? console = null)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(provider => provider.Secrets).Returns([]);
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(obfuscator => obfuscator.Obfuscate(It.IsAny<string?>(), It.IsAny<object?>()))
            .Returns((string? value, object? _) => value ?? string.Empty);
        var nonSpectreLoggerFactory = new Mock<INonSpectreLoggerFactory>();
        nonSpectreLoggerFactory
            .Setup(factory => factory.CreateLoggers(It.IsAny<string>()))
            .Returns([]);
        var loggerControl = new Mock<ISpectreConsoleLoggerControl>();
        loggerControl.SetupGet(control => control.SynchronizationLock).Returns(new object());
        loggerControl
            .Setup(control => control.WouldRender(It.IsAny<string>(), It.IsAny<LogLevel>()))
            .Returns(true);
        loggerControl
            .Setup(control => control.TryAcquireRenderGateAsync(
                It.IsAny<TimeSpan>(),
                It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<IDisposable?>(Mock.Of<IDisposable>()));

        return new ConsoleCoordinator(
            Mock.Of<IBuildSystemFormatterProvider>(),
            Mock.Of<IResultsPrinter>(),
            secretObfuscator.Object,
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()),
            NullLoggerFactory.Instance,
            Mock.Of<IBuildSystemDetector>(),
            Mock.Of<IServiceProvider>(),
            outputCoordinator,
            loggerControl.Object,
            nonSpectreLoggerFactory.Object,
            console ?? DelegatingAnsiConsole.Instance);
    }

    private sealed class RenderingModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
