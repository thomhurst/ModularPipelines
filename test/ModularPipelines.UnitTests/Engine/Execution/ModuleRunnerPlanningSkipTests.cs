using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using Moq;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.Engine.Execution;

public class ModuleRunnerPlanningSkipTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CancelledCallerDoesNotCancelSharedPlanningSkipEvaluation(bool failFast)
    {
        var evaluationStarted = new TaskCompletionSource<CancellationToken>(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseEvaluation = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var evaluationCount = 0;
        var consumer = new ConsumerModule(async (_, token) =>
        {
            Interlocked.Increment(ref evaluationCount);
            evaluationStarted.TrySetResult(token);
            await releaseEvaluation.Task.WaitAsync(token);
            return SkipDecision.Skip("Consumer is disabled");
        });

        var builder = TestPipelineBuilder.Create()
            .AddModule<ProducerModule>()
            .AddModule<AlwaysRunProducerModule>()
            .AddModule(consumer)
            .ConfigureOptions(options => options with { FailureMode = FailureMode.FailFast });
        await using var host = await builder.BuildAsync();
        var runner = host.Services.GetRequiredService<IModuleRunner>();
        var engineCancellation = host.Services.GetRequiredService<EngineCancellationToken>();
        using var callerCancellation = new CancellationTokenSource();
        var callerToken = failFast ? engineCancellation.Token : callerCancellation.Token;
        var scheduler = new Mock<IModuleScheduler>();
        scheduler.Setup(x => x.GetModuleState(typeof(ConsumerModule)))
            .Returns(new ModuleState(consumer, typeof(ConsumerModule)));
        scheduler.Setup(x => x.MarkModuleStarted(typeof(AlwaysRunProducerModule))).Returns(true);
        var producerState = new ModuleState(new ProducerModule(), typeof(ProducerModule));
        var alwaysRunState = new ModuleState(new AlwaysRunProducerModule(), typeof(AlwaysRunProducerModule));

        var first = runner.ExecuteWithoutDependencyWaitAsync(producerState, scheduler.Object, callerToken);
        try
        {
            var evaluationToken = await evaluationStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            if (failFast)
            {
                engineCancellation.CancelWithException(new InvalidOperationException("Another module failed"));
            }
            else
            {
                callerCancellation.Cancel();
            }

            await Assert.That(async () => await first.WaitAsync(TestHostSettings.DefaultTestTimeout))
                .Throws<OperationCanceledException>();

            var second = runner.ExecuteWithoutDependencyWaitAsync(alwaysRunState, scheduler.Object, CancellationToken.None);
            releaseEvaluation.TrySetResult();
            await second.WaitAsync(TestHostSettings.DefaultTestTimeout);

            using (Assert.Multiple())
            {
                await Assert.That(evaluationToken.IsCancellationRequested).IsFalse();
                await Assert.That(evaluationCount).IsEqualTo(1);
                await Assert.That(alwaysRunState.Result!.Status).IsEqualTo(ModuleStatus.Succeeded);
            }
        }
        finally
        {
            releaseEvaluation.TrySetResult();
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task SharedPlanningSkipEvaluationHonorsEngineCancellation(bool shutdown)
    {
        var evaluationStarted = new TaskCompletionSource<CancellationToken>(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseEvaluation = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var consumer = new ConsumerModule(async (_, token) =>
        {
            evaluationStarted.TrySetResult(token);
            await releaseEvaluation.Task.WaitAsync(token);
            return SkipDecision.DoNotSkip;
        });
        var builder = TestPipelineBuilder.Create()
            .AddModule<ProducerModule>()
            .AddModule<AlwaysRunProducerModule>()
            .AddModule(consumer)
            .ConfigureOptions(options => options with { FailureMode = FailureMode.FailFast });
        await using var host = await builder.BuildAsync();
        var runner = host.Services.GetRequiredService<IModuleRunner>();
        var engineCancellation = host.Services.GetRequiredService<EngineCancellationToken>();
        var scheduler = new Mock<IModuleScheduler>();
        scheduler.Setup(x => x.GetModuleState(typeof(ConsumerModule)))
            .Returns(new ModuleState(consumer, typeof(ConsumerModule)));
        var producerState = new ModuleState(new AlwaysRunProducerModule(), typeof(AlwaysRunProducerModule));

        var execution = runner.ExecuteWithoutDependencyWaitAsync(producerState, scheduler.Object, CancellationToken.None);
        try
        {
            var evaluationToken = await evaluationStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            if (shutdown)
            {
                engineCancellation.RequestShutdown();
            }
            else
            {
                engineCancellation.Cancel();
            }

            await Assert.That(evaluationToken.IsCancellationRequested).IsTrue();
            await Assert.That(async () => await execution.WaitAsync(TestHostSettings.DefaultTestTimeout))
                .Throws<OperationCanceledException>();
            scheduler.Verify(x => x.MarkModuleStarted(typeof(AlwaysRunProducerModule)), Times.Never);
        }
        finally
        {
            releaseEvaluation.TrySetResult();
        }
    }

    [ProducesArtifact("output", "unused.txt")]
    private sealed class ProducerModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    [ProducesArtifact("output", "unused.txt")]
    private sealed class AlwaysRunProducerModule : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithAlwaysRun();

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }

    [ConsumesArtifact(typeof(ProducerModule), "output", RestorePath = "unused-first")]
    [ConsumesArtifact(typeof(AlwaysRunProducerModule), "output", RestorePath = "unused-second")]
    [DependsOn<ProducerModule>]
    [DependsOn<AlwaysRunProducerModule>]
    private sealed class ConsumerModule(Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> skipCondition) : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(skipCondition);

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult(true);
    }
}
