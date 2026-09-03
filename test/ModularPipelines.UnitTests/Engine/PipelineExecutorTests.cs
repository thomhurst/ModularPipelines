using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineExecutorTests
{
    [Test]
    public async Task ContinueOnFailure_Does_Not_Throw_Secondary_Exceptions_When_Throwing_Is_Disabled()
    {
        var secondaryExceptions = new Mock<ISecondaryExceptionContainer>();
        var exceptionRethrowService = new Mock<IExceptionRethrowService>();
        var executor = CreateExecutor(
            secondaryExceptions.Object,
            exceptionRethrowService.Object,
            new PipelineOptions
            {
                FailureMode = FailureMode.ContinueOnFailure,
                ThrowOnPipelineFailure = false,
            });

        var summary = await executor.ExecuteAsync([], new OrganizedModules([], []));

        await Assert.That(summary).IsNotNull();
        exceptionRethrowService.Verify(x => x.ThrowOriginalExceptionIfPresent(), Times.Never);
        secondaryExceptions.Verify(x => x.ThrowExceptions(), Times.Never);
    }

    [Test]
    public async Task ContinueOnFailure_Throws_Stored_Exceptions_When_Throwing_Is_Enabled()
    {
        var secondaryExceptions = new Mock<ISecondaryExceptionContainer>();
        var exceptionRethrowService = new Mock<IExceptionRethrowService>();
        var executor = CreateExecutor(
            secondaryExceptions.Object,
            exceptionRethrowService.Object,
            new PipelineOptions
            {
                FailureMode = FailureMode.ContinueOnFailure,
                ThrowOnPipelineFailure = true,
            });

        await executor.ExecuteAsync([], new OrganizedModules([], []));

        exceptionRethrowService.Verify(x => x.ThrowOriginalExceptionIfPresent(), Times.Once);
        secondaryExceptions.Verify(x => x.ThrowExceptions(), Times.Once);
    }

    [Test]
    public async Task Partial_Backend_Does_Not_Require_Results_For_The_Entire_Plan()
    {
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            ownsEntirePlan: false);

        await executor.ExecuteAsync(
            [new UnexecutedModule()],
            new OrganizedModules([], []));
    }

    [Test]
    public async Task Plan_Owning_Backend_Requires_Results_For_The_Entire_Plan()
    {
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            ownsEntirePlan: true);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            executor.ExecuteAsync(
                [new UnexecutedModule()],
                new OrganizedModules([], [])));

        await Assert.That(exception!.Message)
            .Contains("Execution backend completed without results for");
    }

    private static PipelineExecutor CreateExecutor(
        ISecondaryExceptionContainer secondaryExceptions,
        IExceptionRethrowService exceptionRethrowService,
        PipelineOptions options,
        bool ownsEntirePlan = true)
    {
        var executionBackend = new Mock<IExecutionBackend>();
        executionBackend.SetupGet(x => x.OwnsEntirePlan).Returns(ownsEntirePlan);
        executionBackend
            .Setup(x => x.ExecuteAsync(
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<IExecutionBackendContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var executionBackendContext = Mock.Of<IExecutionBackendContext>();
        var engineCancellationToken = new ModularPipelines.Engine.EngineCancellationToken(
            Mock.Of<IPrimaryExceptionContainer>());

        var pipelineSetupExecutor = new Mock<IPipelineSetupExecutor>();
        pipelineSetupExecutor
            .Setup(x => x.OnPipelineEndAsync(It.IsAny<PipelineSummary>()))
            .Returns(Task.CompletedTask);

        var summaryFactory = new Mock<IPipelineSummaryFactory>();
        summaryFactory
            .Setup(x => x.Create(
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<TimeSpan>(),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<DateTimeOffset>()))
            .Returns(new PipelineSummary([], [], TimeSpan.Zero, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow));

        return new PipelineExecutor(
            pipelineSetupExecutor.Object,
            executionBackend.Object,
            executionBackendContext,
            engineCancellationToken,
            NullLogger<PipelineExecutor>.Instance,
            exceptionRethrowService,
            secondaryExceptions,
            summaryFactory.Object,
            Microsoft.Extensions.Options.Options.Create(options));
    }

    private sealed class UnexecutedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("This module must remain unexecuted.");
    }
}
