using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;
using OptionsFactory = Microsoft.Extensions.Options.Options;
using PipelineEngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Engine;

public class ExecutionOrchestratorTests
{
    [Test]
    public async Task FlushesConsoleBeforeCompletingRunReport()
    {
        var organizedModules = new OrganizedModules([], []);
        var summary = new PipelineSummary(
            [],
            [],
            TimeSpan.Zero,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        var pipelineInitializer = new Mock<IPipelineInitializer>();
        pipelineInitializer
            .Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
            .ReturnsAsync(organizedModules);
        var ignoredModuleResultRegistrar = new Mock<IIgnoredModuleResultRegistrar>();
        ignoredModuleResultRegistrar
            .Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules))
            .ReturnsAsync(organizedModules);
        var pipelineExecutor = new Mock<IPipelineExecutor>();
        pipelineExecutor
            .Setup(x => x.ExecuteAsync(It.IsAny<List<IModule>>(), organizedModules))
            .ReturnsAsync(summary);
        var outputScope = new Mock<IPipelineOutputScope>();
        outputScope.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        var outputCoordinator = new Mock<IPipelineOutputCoordinator>();
        outputCoordinator.Setup(x => x.InitializeAsync()).ReturnsAsync(outputScope.Object);
        var consoleWasFlushed = false;
        outputCoordinator
            .Setup(x => x.FlushConsoleAsync())
            .Callback(() => consoleWasFlushed = true)
            .Returns(Task.CompletedTask);
        var moduleDisposeExecutor = new Mock<IModuleDisposeExecutor>();
        moduleDisposeExecutor.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        var wasFlushedBeforeReport = false;
        var runReportService = new Mock<IRunReportService>();
        runReportService
            .Setup(x => x.CompleteAsync(
                It.IsAny<PipelineSummary>(),
                It.IsAny<Exception?>(),
                It.IsAny<CancellationToken>()))
            .Callback(() => wasFlushedBeforeReport = consoleWasFlushed)
            .ReturnsAsync(new PipelineRunReport());
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var orchestrator = new ExecutionOrchestrator(
            pipelineInitializer.Object,
            moduleDisposeExecutor.Object,
            pipelineExecutor.Object,
            outputCoordinator.Object,
            ignoredModuleResultRegistrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            Mock.Of<IPipelineSummaryFactory>(),
            engineCancellationToken,
            Mock.Of<IThreadPoolConfigurator>(),
            Mock.Of<IExceptionRethrowService>(),
            OptionsFactory.Create(new PipelineOptions()),
            Mock.Of<ILogger<ExecutionOrchestrator>>(),
            runReportService.Object);
        await orchestrator.ExecuteAsync();

        await Assert.That(wasFlushedBeforeReport).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CallerCancellationToken_IsPassedToRunReport_AndRegistrationIsDisposed(
        bool hasRecordedFailure)
    {
        var organizedModules = new OrganizedModules([], []);
        var summary = new PipelineSummary(
            [],
            [],
            TimeSpan.Zero,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);

        var pipelineInitializer = new Mock<IPipelineInitializer>();
        pipelineInitializer
            .Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
            .ReturnsAsync(organizedModules);

        var ignoredModuleResultRegistrar = new Mock<IIgnoredModuleResultRegistrar>();
        ignoredModuleResultRegistrar
            .Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules))
            .ReturnsAsync(organizedModules);

        var pipelineExecutor = new Mock<IPipelineExecutor>();
        pipelineExecutor
            .Setup(x => x.ExecuteAsync(It.IsAny<List<IModule>>(), organizedModules))
            .ReturnsAsync(summary);

        var outputScope = new Mock<IPipelineOutputScope>();
        outputScope
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask);

        var outputCoordinator = new Mock<IPipelineOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.InitializeAsync())
            .ReturnsAsync(outputScope.Object);

        var moduleDisposeExecutor = new Mock<IModuleDisposeExecutor>();
        moduleDisposeExecutor
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask);

        var runReportCancellationToken = CancellationToken.None;
        var runReportService = new Mock<IRunReportService>();
        runReportService.Setup(x => x.CompleteAsync(
                It.IsAny<PipelineSummary>(),
                It.IsAny<Exception?>(),
                It.IsAny<CancellationToken>()))
            .Callback<PipelineSummary, Exception?, CancellationToken>((_, _, token) =>
                runReportCancellationToken = token)
            .ReturnsAsync(new PipelineRunReport());

        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        if (hasRecordedFailure)
        {
            engineCancellationToken.RecordException(new InvalidOperationException("module failed"));
        }

        var orchestrator = new ExecutionOrchestrator(
            pipelineInitializer.Object,
            moduleDisposeExecutor.Object,
            pipelineExecutor.Object,
            outputCoordinator.Object,
            ignoredModuleResultRegistrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            Mock.Of<IPipelineSummaryFactory>(),
            engineCancellationToken,
            Mock.Of<IThreadPoolConfigurator>(),
            Mock.Of<IExceptionRethrowService>(),
            OptionsFactory.Create(new PipelineOptions()),
            Mock.Of<ILogger<ExecutionOrchestrator>>(),
            runReportService.Object);
        using var callerCancellationTokenSource = new CancellationTokenSource();

        await orchestrator.ExecuteAsync(callerCancellationTokenSource.Token);
        callerCancellationTokenSource.Cancel();

        using (Assert.Multiple())
        {
            await Assert.That(engineCancellationToken.IsCancellationRequested).IsFalse();
            await Assert.That(runReportCancellationToken.CanBeCanceled).IsTrue();
            await Assert.That(runReportCancellationToken.IsCancellationRequested).IsFalse();
        }
    }

    [Test]
    public async Task PipelineFailure_IsNotMaskedByOutputTeardownOrLoggingFailure()
    {
        var organizedModules = new OrganizedModules([], []);
        var summary = new PipelineSummary(
            [],
            [],
            TimeSpan.Zero,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        var pipelineException = new InvalidOperationException("pipeline failed");
        var teardownException = new IOException("output teardown failed");
        var loggingException = new InvalidOperationException("logging failed");

        var pipelineInitializer = new Mock<IPipelineInitializer>();
        pipelineInitializer
            .Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
            .ReturnsAsync(organizedModules);

        var ignoredModuleResultRegistrar = new Mock<IIgnoredModuleResultRegistrar>();
        ignoredModuleResultRegistrar
            .Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules))
            .ReturnsAsync(organizedModules);

        var pipelineExecutor = new Mock<IPipelineExecutor>();
        pipelineExecutor
            .Setup(x => x.ExecuteAsync(It.IsAny<List<IModule>>(), organizedModules))
            .ThrowsAsync(pipelineException);

        var outputScope = new Mock<IPipelineOutputScope>();
        outputScope
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.FromException(teardownException));

        var outputCoordinator = new Mock<IPipelineOutputCoordinator>();
        outputCoordinator
            .Setup(x => x.InitializeAsync())
            .ReturnsAsync(outputScope.Object);

        var moduleDisposeExecutor = new Mock<IModuleDisposeExecutor>();
        moduleDisposeExecutor
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask);

        var pipelineSummaryFactory = new Mock<IPipelineSummaryFactory>();
        pipelineSummaryFactory
            .Setup(x => x.Create(
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .Returns(summary);

        var primaryExceptionContainer = new Mock<IPrimaryExceptionContainer>();
        using var engineCancellationToken = new PipelineEngineCancellationToken(primaryExceptionContainer.Object);
        var logger = new Mock<ILogger<ExecutionOrchestrator>>();
        logger
            .Setup(x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()))
            .Throws(loggingException);
        var orchestrator = new ExecutionOrchestrator(
            pipelineInitializer.Object,
            moduleDisposeExecutor.Object,
            pipelineExecutor.Object,
            outputCoordinator.Object,
            ignoredModuleResultRegistrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            pipelineSummaryFactory.Object,
            engineCancellationToken,
            Mock.Of<IThreadPoolConfigurator>(),
            Mock.Of<IExceptionRethrowService>(),
            OptionsFactory.Create(new PipelineOptions()),
            logger.Object,
            CreateRunReportService());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => orchestrator.ExecuteAsync());

        await Assert.That(exception).IsSameReferenceAs(pipelineException);
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state.ToString()!.Contains(
                    nameof(InvalidOperationException),
                    StringComparison.Ordinal)),
            teardownException,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task InitializationFailure_IsIncludedInRunReport(bool initializerFails)
    {
        var module = Mock.Of<IModule>();
        var organizedModules = new OrganizedModules([new RunnableModule(module, TimeSpan.Zero)], []);
        var initializationException = new InvalidOperationException("initialization failed");
        var summary = new PipelineSummary(
            [module],
            [],
            TimeSpan.Zero,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        var pipelineInitializer = new Mock<IPipelineInitializer>();
        pipelineInitializer
            .SetupGet(x => x.RegisteredModules)
            .Returns([module]);
        var ignoredModuleResultRegistrar = new Mock<IIgnoredModuleResultRegistrar>();
        if (initializerFails)
        {
            pipelineInitializer
                .Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
                .ThrowsAsync(initializationException);
        }
        else
        {
            pipelineInitializer
                .Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
                .ReturnsAsync(organizedModules);
            ignoredModuleResultRegistrar
                .Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules))
                .ThrowsAsync(initializationException);
        }

        var pipelineSummaryFactory = new Mock<IPipelineSummaryFactory>();
        pipelineSummaryFactory
            .Setup(x => x.Create(
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .Returns(summary);
        var runReportService = new Mock<IRunReportService>();
        runReportService
            .Setup(x => x.CompleteAsync(
                summary,
                initializationException,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PipelineRunReport { Status = Status.Failed });
        var outputCoordinator = new Mock<IPipelineOutputCoordinator>();
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var orchestrator = new ExecutionOrchestrator(
            pipelineInitializer.Object,
            Mock.Of<IModuleDisposeExecutor>(),
            Mock.Of<IPipelineExecutor>(),
            outputCoordinator.Object,
            ignoredModuleResultRegistrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            pipelineSummaryFactory.Object,
            engineCancellationToken,
            Mock.Of<IThreadPoolConfigurator>(),
            Mock.Of<IExceptionRethrowService>(),
            OptionsFactory.Create(new PipelineOptions()),
            Mock.Of<ILogger<ExecutionOrchestrator>>(),
            runReportService.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => orchestrator.ExecuteAsync());

        await Assert.That(exception).IsSameReferenceAs(initializationException);
        runReportService.Verify(
            x => x.CompleteAsync(
                summary,
                initializationException,
                It.IsAny<CancellationToken>()),
            Times.Once);
        pipelineSummaryFactory.Verify(x => x.Create(
            It.Is<IReadOnlyList<IModule>>(modules => modules.Count == 1 && ReferenceEquals(modules[0], module)),
            It.IsAny<TimeSpan>(),
            It.IsAny<DateTimeOffset>(),
            It.IsAny<DateTimeOffset>()), Times.Once);
        outputCoordinator.Verify(x => x.PrintResults(
            It.Is<PipelineSummary>(printedSummary => printedSummary.Status == Status.Failed)), Times.Once);
    }

    private static IRunReportService CreateRunReportService()
    {
        var service = new Mock<IRunReportService>();
        service.Setup(x => x.CompleteAsync(
                It.IsAny<PipelineSummary>(),
                It.IsAny<Exception?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PipelineRunReport());
        return service.Object;
    }
}
