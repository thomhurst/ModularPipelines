using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Events;
using ModularPipelines.Exceptions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Requirements;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Engine;

public class PipelineEventLifecycleTests
{
    [Test]
    public async Task BackendFailure_EndFailure_DoesNotReplaceOriginalException()
    {
        var handler = new RecordingHandler { EndFails = true };
        var backendException = new InvalidOperationException("backend failed");
        var backend = new Mock<IExecutionBackend>();
        backend.SetupGet(x => x.OwnsEntirePlan).Returns(true);
        backend.Setup(x => x.ExecuteAsync(It.IsAny<ExecutionBackendRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(backendException);
        var builder = CreateBuilder(handler);
        builder.Services.AddSingleton(backend.Object);
        await using var pipeline = await builder.BuildAsync();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => pipeline.RunAsync());

        await Assert.That(exception).IsSameReferenceAs(backendException);
        await AssertFailedLifecycle(handler);
        var secondaryExceptions = pipeline.Services.GetRequiredService<ISecondaryExceptionContainer>();
        var secondaryException = Assert.Throws<InvalidOperationException>(() => secondaryExceptions.ThrowExceptions());
        await Assert.That(secondaryException!.Message).IsEqualTo("end failed");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RequirementFailure_RunsEndOnce_WithFailedSummary_AndPreservesFailure(bool endFails)
    {
        var handler = new RecordingHandler { EndFails = endFails };
        var builder = CreateBuilder(handler).AddRequirement<FailingRequirement>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.ThrowsAsync<RequirementNotMetException>(() => pipeline.RunAsync());

        await AssertFailedLifecycle(handler);
        await Assert.That(handler.Summary!.Modules).HasSingleItem();
        if (endFails)
        {
            var secondaryExceptions = pipeline.Services.GetRequiredService<ISecondaryExceptionContainer>();
            var exception = Assert.Throws<InvalidOperationException>(() => secondaryExceptions.ThrowExceptions());
            await Assert.That(exception!.Message).IsEqualTo("end failed");
        }
    }

    [Test]
    public async Task RunConditionFailure_RunsEndOnce_WithFailedSummary()
    {
        var handler = new RecordingHandler();
        await using var pipeline = await CreateBuilder(handler)
            .AddModule<ThrowingConditionModule>()
            .BuildAsync();

        var exception = await Assert.ThrowsAsync<ModuleFailedException>(() => pipeline.RunAsync());
        await Assert.That(exception!.InnerException!.Message).IsEqualTo("condition failed");

        await AssertFailedLifecycle(handler);
    }

    [Test]
    public async Task StartHandlerFailure_RunsAllEndHandlers_AndPreservesFailure()
    {
        var handler = new RecordingHandler { StartFails = true, EndFails = true };
        var secondHandler = new RecordingHandler();
        var builder = CreateBuilder(handler);
        builder.Services.AddSingleton<IPipelineEventHandler>(secondHandler);
        await using var pipeline = await builder.BuildAsync();

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => pipeline.RunAsync());

        await Assert.That(exception!.Message).IsEqualTo("start failed");
        await AssertFailedLifecycle(handler);
        await AssertFailedLifecycle(secondHandler);
    }

    [Test]
    public async Task ConsoleCancellationDuringStartup_RunsEndOnce_WithFailedSummary()
    {
        var handler = new RecordingHandler();
        await using var pipeline = await CreateBuilder(handler)
            .AddRequirement<CancelingRequirement>()
            .BuildAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(() => pipeline.RunAsync());

        await AssertFailedLifecycle(handler);
        await Assert.That(handler.EndToken.CanBeCanceled).IsTrue();
        await Assert.That(handler.EndToken.IsCancellationRequested).IsFalse();
    }

    [Test]
    public async Task FailureBeforeStart_DoesNotRunEnd()
    {
        var handler = new RecordingHandler();
        var writer = new Mock<IPipelineFileWriter>();
        writer.Setup(x => x.WritePipelineFiles()).ThrowsAsync(new InvalidOperationException("file write failed"));
        var builder = CreateBuilder(handler);
        builder.Services.AddSingleton(writer.Object);
        await using var pipeline = await builder.BuildAsync();

        await Assert.ThrowsAsync<InvalidOperationException>(() => pipeline.RunAsync());

        await Assert.That(handler.StartCount).IsEqualTo(0);
        await Assert.That(handler.EndCount).IsEqualTo(0);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CompletedPipeline_RunsEndExactlyOnce(bool endFails)
    {
        var handler = new RecordingHandler { EndFails = endFails };
        await using var pipeline = await CreateBuilder(handler).BuildAsync();

        if (endFails)
        {
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => pipeline.RunAsync());
            await Assert.That(exception!.Message).IsEqualTo("end failed");
        }
        else
        {
            await pipeline.RunAsync();
        }

        await Assert.That(handler.StartCount).IsEqualTo(1);
        await Assert.That(handler.EndCount).IsEqualTo(1);
        await Assert.That(handler.Summary!.Succeeded).IsTrue();
    }

    private static PipelineBuilder CreateBuilder(RecordingHandler handler)
    {
        var builder = TestPipelineBuilder.Create().AddModule<SuccessfulModule>();
        builder.Services.AddSingleton<IPipelineEventHandler>(handler);
        return builder;
    }

    private static async Task AssertFailedLifecycle(RecordingHandler handler)
    {
        await Assert.That(handler.StartCount).IsEqualTo(1);
        await Assert.That(handler.EndCount).IsEqualTo(1);
        await Assert.That(handler.Summary!.Status).IsEqualTo(ModuleStatus.Failed);
    }

    private sealed class RecordingHandler : IPipelineEventHandler
    {
        public bool StartFails { get; init; }

        public bool EndFails { get; init; }

        public int StartCount { get; private set; }

        public int EndCount { get; private set; }

        public PipelineSummary? Summary { get; private set; }

        public CancellationToken EndToken { get; private set; }

        public Task OnPipelineStartAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            StartCount++;
            return StartFails ? Task.FromException(new InvalidOperationException("start failed")) : Task.CompletedTask;
        }

        public Task OnPipelineEndAsync(IPipelineContext context, PipelineSummary pipelineSummary, CancellationToken cancellationToken)
        {
            EndCount++;
            Summary = pipelineSummary;
            EndToken = cancellationToken;
            return EndFails ? Task.FromException(new InvalidOperationException("end failed")) : Task.CompletedTask;
        }
    }

    private sealed class FailingRequirement : IPipelineRequirement
    {
        public Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            Task.FromResult(RequirementDecision.Failed("startup requirement failed"));
    }

    private sealed class CancelingRequirement(ModularPipelines.Engine.EngineCancellationToken engineCancellationToken) : IPipelineRequirement
    {
        public Task<RequirementDecision> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            engineCancellationToken.HandleCancelKeyPress();
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(RequirementDecision.Passed);
        }
    }

    private sealed class SuccessfulModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("success");
    }

    private sealed class ThrowingCondition : IRunCondition, IPlanningSafe
    {
        public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("condition failed");
    }

    [RunIf<ThrowingCondition>]
    private sealed class ThrowingConditionModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Module must not execute.");
    }
}
