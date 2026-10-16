using ModularPipelines.Reporting;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
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
    [Arguments(false)]
    [Arguments(true)]
    public async Task EndHandlerCancellation_PreservesFirstFailure(bool executionFails)
    {
        var organizedModules = new OrganizedModules([], []);
        var summary = new PipelineSummary([], [], TimeSpan.Zero, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        var initializer = new Mock<IPipelineInitializer>();
        initializer.Setup(x => x.Initialize(It.IsAny<CancellationToken>())).ReturnsAsync(organizedModules);
        var registrar = new Mock<IIgnoredModuleResultRegistrar>();
        registrar.Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules)).ReturnsAsync(organizedModules);
        var executionException = new InvalidOperationException("execution failed");
        var executor = new Mock<IPipelineExecutor>();
        var execution = executor.Setup(x => x.ExecuteAsync(It.IsAny<List<IModule>>(), organizedModules));
        if (executionFails)
        {
            execution.ThrowsAsync(executionException);
        }
        else
        {
            execution.ReturnsAsync(summary);
        }

        var summaryFactory = new Mock<IPipelineSummaryFactory>();
        summaryFactory.Setup(x => x.Create(It.IsAny<IReadOnlyList<IModule>>(), It.IsAny<TimeSpan>(),
                It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>()))
            .Returns(summary);
        var output = new Mock<IPipelineOutputCoordinator>();
        output.Setup(x => x.InitializeAsync()).ReturnsAsync(Mock.Of<IPipelineOutputScope>());
        using var engineCancellation = new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var cancellationException = new OperationCanceledException(engineCancellation.NonFailureCancellationToken);
        var setup = new Mock<IPipelineSetupExecutor>();
        setup.SetupGet(x => x.PipelineStartInvoked).Returns(true);
        setup.Setup(x => x.OnPipelineEndAsync(It.IsAny<PipelineSummary>(), It.IsAny<CancellationToken>()))
            .Callback(() => engineCancellation.Cancel())
            .ThrowsAsync(cancellationException);
        var orchestrator = new ExecutionOrchestrator(
            initializer.Object,
            executor.Object,
            output.Object,
            registrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            summaryFactory.Object,
            engineCancellation,
            Mock.Of<IThreadPoolConfigurator>(),
            Mock.Of<IExceptionRethrowService>(),
            OptionsFactory.Create(new PipelineOptions()),
            Mock.Of<ILogger<ExecutionOrchestrator>>(),
            CreateRunReportService(),
            setup.Object,
            Mock.Of<ISecondaryExceptionContainer>());

        if (executionFails)
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => orchestrator.ExecuteAsync());
            await Assert.That(exception).IsSameReferenceAs(executionException);
        }
        else
        {
            var exception = await Assert.ThrowsAsync<OperationCanceledException>(() => orchestrator.ExecuteAsync());
            await Assert.That(exception).IsSameReferenceAs(cancellationException);
        }

        output.Verify(x => x.PrintResults(It.Is<PipelineSummary>(result => result.Status == ModuleStatus.Failed)), Times.Once);
        setup.Verify(x => x.OnPipelineEndAsync(It.IsAny<PipelineSummary>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [Arguments(ModuleStatus.Failed, true)]
    [Arguments(ModuleStatus.Failed, false)]
    [Arguments(ModuleStatus.TimedOut, true)]
    [Arguments(ModuleStatus.TimedOut, false)]
    [Arguments(ModuleStatus.DependencyFailed, true)]
    [Arguments(ModuleStatus.DependencyFailed, false)]
    [Arguments(ModuleStatus.Canceled, true)]
    [Arguments(ModuleStatus.Canceled, false)]
    public async Task FailureStatusWithoutException_RespectsThrowOnPipelineFailure(
        ModuleStatus status,
        bool throwOnFailure)
    {
        var module = Mock.Of<IModule>();
        var ignoredModule = Mock.Of<IModule>();
        var organizedModules = new OrganizedModules(
            [new RunnableModule(module, TimeSpan.Zero), new RunnableModule(ignoredModule, TimeSpan.Zero)], []);
        var result = Mock.Of<IModuleResult>(x => x.Status == status && x.Name == "StatusOnlyFailure");
        var ignoredResult = Mock.Of<IModuleResult>(x => x.Status == ModuleStatus.FailureIgnored
            && x.Name == "IgnoredFailure" && x.ExceptionOrDefault == new InvalidOperationException("ignored"));
        var summary = new PipelineSummary(
            [module, ignoredModule], [result, ignoredResult], TimeSpan.Zero, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
        var initializer = new Mock<IPipelineInitializer>();
        initializer.Setup(x => x.Initialize(It.IsAny<CancellationToken>())).ReturnsAsync(organizedModules);
        var registrar = new Mock<IIgnoredModuleResultRegistrar>();
        registrar.Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules)).ReturnsAsync(organizedModules);
        var executor = new Mock<IPipelineExecutor>();
        executor.Setup(x => x.ExecuteAsync(It.IsAny<List<IModule>>(), organizedModules)).ReturnsAsync(summary);
        var output = new Mock<IPipelineOutputCoordinator>();
        output.Setup(x => x.InitializeAsync()).ReturnsAsync(Mock.Of<IPipelineOutputScope>());
        using var engineCancellationToken = new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var orchestrator = new ExecutionOrchestrator(
            initializer.Object,
            executor.Object,
            output.Object,
            registrar.Object,
            Mock.Of<IModuleResultRegistry>(),
            Mock.Of<IPipelineSummaryFactory>(),
            engineCancellationToken,
            Mock.Of<IThreadPoolConfigurator>(),
            Mock.Of<IExceptionRethrowService>(),
            OptionsFactory.Create(new PipelineOptions { ThrowOnPipelineFailure = throwOnFailure }),
            Mock.Of<ILogger<ExecutionOrchestrator>>(),
            CreateRunReportService(),
            Mock.Of<IPipelineSetupExecutor>(),
            Mock.Of<ISecondaryExceptionContainer>());

        if (throwOnFailure)
        {
            var exception = (await Assert.ThrowsAsync<PipelineFailedException>(() => orchestrator.ExecuteAsync()))!;
            await Assert.That(exception.Summary.Succeeded).IsFalse();
            await Assert.That(exception.Summary.Failures).IsEquivalentTo([result]);
            await Assert.That(exception.Summary.IgnoredFailures).IsEquivalentTo([ignoredResult]);
            await Assert.That(exception.FailedModules).IsEquivalentTo(["StatusOnlyFailure"]);
        }
        else
        {
            var returnedSummary = await orchestrator.ExecuteAsync();
            await Assert.That(returnedSummary.Succeeded).IsFalse();
            await Assert.That(returnedSummary.Failures).IsEquivalentTo([result]);
            await Assert.That(returnedSummary.IgnoredFailures).IsEquivalentTo([ignoredResult]);
        }
    }

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
        var initializerCancellationToken = CancellationToken.None;
        pipelineInitializer
            .Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
            .Callback<CancellationToken>(token => initializerCancellationToken = token)
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
            runReportService.Object,
            Mock.Of<IPipelineSetupExecutor>(),
            Mock.Of<ISecondaryExceptionContainer>());
        await orchestrator.ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(wasFlushedBeforeReport).IsTrue();
            await Assert.That(initializerCancellationToken).IsEqualTo(engineCancellationToken.Token);
        }
    }

    [Test]
    public async Task ConsoleFlushFailureDoesNotPreventRunReportCompletion()
    {
        var organizedModules = new OrganizedModules([], []);
        var summary = new PipelineSummary(
            [],
            [],
            TimeSpan.Zero,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow);
        var pipelineInitializer = new Mock<IPipelineInitializer>();
        pipelineInitializer.Setup(x => x.Initialize(It.IsAny<CancellationToken>()))
            .ReturnsAsync(organizedModules);
        var ignoredModuleResultRegistrar = new Mock<IIgnoredModuleResultRegistrar>();
        ignoredModuleResultRegistrar.Setup(x => x.RegisterIgnoredModuleResultsAsync(organizedModules))
            .ReturnsAsync(organizedModules);
        var pipelineExecutor = new Mock<IPipelineExecutor>();
        pipelineExecutor.Setup(x => x.ExecuteAsync(It.IsAny<List<IModule>>(), organizedModules))
            .ReturnsAsync(summary);
        var outputScope = new Mock<IPipelineOutputScope>();
        outputScope.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);
        var outputCoordinator = new Mock<IPipelineOutputCoordinator>();
        outputCoordinator.Setup(x => x.InitializeAsync()).ReturnsAsync(outputScope.Object);
        outputCoordinator.SetupSequence(x => x.FlushConsoleAsync())
            .Returns(Task.FromException(new IOException("broken console")))
            .Returns(Task.CompletedTask);
        var runReportService = new Mock<IRunReportService>();
        runReportService.Setup(x => x.CompleteAsync(
                It.IsAny<PipelineSummary>(),
                It.IsAny<Exception?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PipelineRunReport());
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var orchestrator = new ExecutionOrchestrator(
            pipelineInitializer.Object,
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
            runReportService.Object,
            Mock.Of<IPipelineSetupExecutor>(),
            Mock.Of<ISecondaryExceptionContainer>());

        await orchestrator.ExecuteAsync();

        runReportService.Verify(x => x.CompleteAsync(
            It.IsAny<PipelineSummary>(),
            It.IsAny<Exception?>(),
            It.IsAny<CancellationToken>()), Times.Once);
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
            runReportService.Object,
            Mock.Of<IPipelineSetupExecutor>(),
            Mock.Of<ISecondaryExceptionContainer>());
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
            CreateRunReportService(),
            Mock.Of<IPipelineSetupExecutor>(),
            Mock.Of<ISecondaryExceptionContainer>());

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => orchestrator.ExecuteAsync());

        await Assert.That(exception).IsSameReferenceAs(pipelineException);
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((state, _) =>
                state!.ToString()!.Contains(
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
            .ReturnsAsync(new PipelineRunReport { Status = ModuleStatus.Failed });
        var outputCoordinator = new Mock<IPipelineOutputCoordinator>();
        using var engineCancellationToken =
            new PipelineEngineCancellationToken(new PrimaryExceptionContainer());
        var orchestrator = new ExecutionOrchestrator(
            pipelineInitializer.Object,
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
            runReportService.Object,
            Mock.Of<IPipelineSetupExecutor>(),
            Mock.Of<ISecondaryExceptionContainer>());

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
            It.Is<PipelineSummary>(printedSummary => printedSummary.Status == ModuleStatus.Failed)), Times.Once);
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
