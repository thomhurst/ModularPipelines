using Microsoft.Extensions.Logging;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
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
    public async Task PipelineFailure_IsNotMaskedByOutputTeardownOrLoggingFailure()
    {
        var organizedModules = new OrganizedModules([], []);
        var summary = new PipelineSummary(
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
            logger.Object);

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
}
