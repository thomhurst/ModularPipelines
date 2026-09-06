using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Enums;
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

    [Test]
    public async Task Backend_Result_Replay_Is_Idempotent()
    {
        var module = new UnexecutedModule();
        var result = CreateResult(module, "first");
        ModuleCompletionSourceApplicator.TryApply(module, result);
        var context = new Mock<IExecutionBackendContext>();
        context.Setup(x => x.TryApplyResult(module, result)).Returns(false);
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            backendResults: [result],
            executionBackendContext: context.Object);

        await executor.ExecuteAsync([module], new OrganizedModules([], []));
    }

    [Test]
    public async Task Backend_Rejects_Conflicting_Result_For_Completed_Module()
    {
        var module = new UnexecutedModule();
        var acceptedResult = CreateResult(module, "first");
        var conflictingResult = CreateResult(module, "second");
        ModuleCompletionSourceApplicator.TryApply(module, acceptedResult);
        var context = new Mock<IExecutionBackendContext>();
        context.Setup(x => x.TryApplyResult(module, conflictingResult)).Returns(false);
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            backendResults: [conflictingResult],
            executionBackendContext: context.Object);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            executor.ExecuteAsync([module], new OrganizedModules([], [])));

        await Assert.That(exception!.Message).Contains("conflicting result");
        await Assert.That(await module).IsSameReferenceAs(acceptedResult);
    }

    [Test]
    public async Task Backend_Result_Requires_Fully_Qualified_Type_Name()
    {
        var module = new UnexecutedModule();
        var result = new Mock<IModuleResult>();
        result.SetupGet(x => x.Name).Returns(module.GetType().Name);
        result.SetupGet(x => x.TypeName).Returns((string?) null);
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            backendResults: [result.Object]);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            executor.ExecuteAsync([module], new OrganizedModules([], [])));

        await Assert.That(exception!.Message).Contains("fully qualified TypeName");
    }

    [Test]
    public async Task Backend_Results_Resolve_Their_Owning_Module_When_Type_Names_Collide()
    {
        var first = new UnexecutedModule();
        var second = new UnexecutedModule();
        var firstResult = CreateResult(first, "first");
        var secondResult = CreateResult(second, "second");
        ModuleCompletionSourceApplicator.TryApply(first, firstResult);
        ModuleCompletionSourceApplicator.TryApply(second, secondResult);
        var context = new Mock<IExecutionBackendContext>();
        context
            .Setup(x => x.TryApplyResult(It.IsAny<IModule>(), It.IsAny<IModuleResult>()))
            .Returns(false);
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            backendResults: [firstResult, secondResult],
            executionBackendContext: context.Object);

        await executor.ExecuteAsync([first, second], new OrganizedModules([], []));

        using (Assert.Multiple())
        {
            await Assert.That(await first).IsSameReferenceAs(firstResult);
            await Assert.That(await second).IsSameReferenceAs(secondResult);
        }

        context.Verify(x => x.TryApplyResult(first, firstResult), Times.Once);
        context.Verify(x => x.TryApplyResult(second, secondResult), Times.Once);
    }

    [Test]
    public async Task Foreign_Backend_Result_With_Ambiguous_Type_Name_Is_Rejected()
    {
        var first = new UnexecutedModule();
        var second = new UnexecutedModule();
        var foreignResult = CreateResult(first, "foreign");
        var executor = CreateExecutor(
            Mock.Of<ISecondaryExceptionContainer>(),
            Mock.Of<IExceptionRethrowService>(),
            new PipelineOptions { FailureMode = FailureMode.ContinueOnFailure },
            backendResults: [foreignResult]);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            executor.ExecuteAsync([first, second], new OrganizedModules([], [])));

        await Assert.That(exception!.Message).Contains("matched 2 planned modules");
    }

    private static PipelineExecutor CreateExecutor(
        ISecondaryExceptionContainer secondaryExceptions,
        IExceptionRethrowService exceptionRethrowService,
        PipelineOptions options,
        bool ownsEntirePlan = true,
        IReadOnlyList<IModuleResult>? backendResults = null,
        IExecutionBackendContext? executionBackendContext = null)
    {
        var executionBackend = new Mock<IExecutionBackend>();
        executionBackend.SetupGet(x => x.OwnsEntirePlan).Returns(ownsEntirePlan);
        executionBackend
            .Setup(x => x.ExecuteAsync(
                It.IsAny<IReadOnlyList<IModule>>(),
                It.IsAny<IExecutionBackendContext>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(backendResults ?? []);
        executionBackendContext ??= Mock.Of<IExecutionBackendContext>();
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

    private static ModuleResult<string> CreateResult(IModule module, string value)
    {
        var now = DateTimeOffset.UtcNow;
        return new ModuleResult<string>.Success(value)
        {
            Name = module.GetType().Name,
            TypeName = module.GetType().FullName,
            StartTime = now,
            EndTime = now,
            Duration = TimeSpan.Zero,
            Status = ModuleStatus.Succeeded,
        };
    }

    private sealed class UnexecutedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("This module must remain unexecuted.");
    }
}
