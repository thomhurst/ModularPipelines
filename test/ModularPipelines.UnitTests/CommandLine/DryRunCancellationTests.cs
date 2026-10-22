using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using EngineCancellationToken = ModularPipelines.Engine.EngineCancellationToken;

namespace ModularPipelines.UnitTests.PipelineCli;

public class DryRunCancellationTests
{
    [Test]
    [Arguments(true, false)]
    [Arguments(true, true)]
    [Arguments(false, true)]
    public async Task DryRunCancelsPlanningWhenConsoleOrCallerCancels(bool consoleCancellation, bool useCallerToken)
    {
        var started = new TaskCompletionSource<CancellationToken>(TaskCreationOptions.RunContinuationsAsynchronously);
        var release = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var module = new WaitingSkipModule(async (_, token) =>
        {
            started.TrySetResult(token);
            await release.Task.WaitAsync(token);
            return SkipDecision.DoNotSkip;
        });
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModule(module)
            .ConfigureOptions(options => options with { DryRun = true })
            .BuildAsync();
        using var callerCancellation = new CancellationTokenSource();
        var run = pipeline.RunAsync(useCallerToken ? callerCancellation.Token : CancellationToken.None);
        try
        {
            var planningToken = await started.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            if (consoleCancellation)
            {
                var engineCancellation = pipeline.Services.GetRequiredService<EngineCancellationToken>();
                await Assert.That(engineCancellation.HandleCancelKeyPress()).IsTrue();
            }
            else
            {
                callerCancellation.Cancel();
            }

            await Assert.That(planningToken.IsCancellationRequested).IsTrue();
            await Assert.That(async () => await run.WaitAsync(TestHostSettings.DefaultTestTimeout))
                .Throws<OperationCanceledException>();
        }
        finally
        {
            release.TrySetResult();
            try
            {
                await run.WaitAsync(TestHostSettings.DefaultTestTimeout);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }

    private sealed class WaitingSkipModule(Func<IModuleContext, CancellationToken, ValueTask<SkipDecision>> skipCondition)
        : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(skipCondition);

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }
}
