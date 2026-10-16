using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Hooks;

public class AfterExecuteReplacementTests
{
    [Test]
    public async Task FreshReplacement_PreservesIdentityAndTiming_AndDerivesStatus()
    {
        var host = await TestPipelineBuilder.Create().AddModule<FreshReplacementModule>().BuildAsync();
        var summary = await host.RunAsync();
        var module = host.Services.GetServices<IModule>().OfType<FreshReplacementModule>().Single();
        var result = summary.GetResult<FreshReplacementModule, string>();
        var original = module.Original!;
        var report = host.Services.GetRequiredService<PipelineRunReportFactory>()
            .Create(summary, null, "replacement-result").Modules.Single();

        using (Assert.Multiple())
        {
            await Assert.That(result.Value).IsEqualTo("replacement");
            await Assert.That(result.Name).IsEqualTo(original.Name);
            await Assert.That(result.TypeName).IsEqualTo(original.TypeName);
            await Assert.That(result.ModuleType).IsEqualTo(typeof(FreshReplacementModule));
            await Assert.That(result.WorkerId).IsEqualTo(original.WorkerId);
            await Assert.That(result.StartTime).IsEqualTo(original.StartTime);
            await Assert.That(result.EndTime).IsEqualTo(original.EndTime);
            await Assert.That(result.Duration).IsEqualTo(original.Duration);
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(await module).IsSameReferenceAs(result);
            await Assert.That(report.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(report.ResultStartTime).IsEqualTo(original.StartTime);
            await Assert.That(report.ResultEndTime).IsEqualTo(original.EndTime);
        }
    }

    [Test]
    public async Task FailureRecovery_ReachesSummaryAndDependent_BeforeFailureIsPublished()
    {
        var summary = await TestPipelineBuilder.Create()
            .AddModule<RecoveringModule>()
            .AddModule<RecoveryDependentModule>()
            .RunAsync();

        await Assert.That(summary.GetResult<RecoveringModule, string>().Value).IsEqualTo("recovered");
        await Assert.That(summary.GetResult<RecoveringModule, string>().Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(summary.GetResult<RecoveryDependentModule, string>().Value).IsEqualTo("recovered");
        await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task IgnoredFailureRecovery_PublishesRecoveredResult()
    {
        var summary = await TestPipelineBuilder.Create().AddModule<IgnoredRecoveringModule>().RunAsync();

        await Assert.That(summary.GetResult<IgnoredRecoveringModule, string>().Value).IsEqualTo("recovered");
        await Assert.That(summary.GetResult<IgnoredRecoveringModule, string>().Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(summary.Status).IsEqualTo(ModuleStatus.Succeeded);
    }

    [Test]
    public async Task SuccessReplacedByFailure_AppliesFailurePolicy()
    {
        var host = await TestPipelineBuilder.Create().AddModule<FailureReplacementModule>().BuildAsync();

        await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());
        var result = await host.Services.GetServices<IModule>().OfType<FailureReplacementModule>().Single();
        await Assert.That(result).IsTypeOf<ModuleResult<string>.Failure>();
        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
        await Assert.That(result.ExceptionOrDefault!.Message).IsEqualTo("replacement failure");
    }

    [Test]
    public async Task SuccessReplacedBySkipped_DerivesSkippedStatus()
    {
        var summary = await TestPipelineBuilder.Create().AddModule<SkippedReplacementModule>().RunAsync();
        var result = summary.GetResult<SkippedReplacementModule, string>();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(result.SkipDecisionOrDefault!.Reason).IsEqualTo("replacement skip");
    }

    [Test]
    public async Task FailureReplacedByFailure_PreservesIgnoredClassification()
    {
        var summary = await TestPipelineBuilder.Create().AddModule<IgnoredFailureReplacementModule>().RunAsync();
        var result = summary.GetResult<IgnoredFailureReplacementModule, string>();

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.FailureIgnored);
        await Assert.That(result.ExceptionOrDefault!.Message).IsEqualTo("replacement failure");
    }

    private class FreshReplacementModule : Module<string>
    {
        public ModuleResult<string>? Original { get; private set; }

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult("original");

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
        {
            Original = result;
            return Task.FromResult<ModuleResult<string>?>(new ModuleResult<string>.Success("replacement")
            {
                Name = "incorrect",
                TypeName = null,
                ModuleType = typeof(string),
                WorkerId = new WorkerId("incorrect-worker"),
                StartTime = DateTimeOffset.MinValue,
                EndTime = DateTimeOffset.MaxValue,
                Duration = TimeSpan.Zero,
                Status = ModuleStatus.Failed,
            });
        }
    }

    private class RecoveringModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("original failure");

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
            => Task.FromResult<ModuleResult<string>?>(result.ToSuccess("recovered"));
    }

    private class IgnoredRecoveringModule : RecoveringModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithIgnoreFailures();
    }

    [ModularPipelines.DependsOn<RecoveringModule>]
    private class RecoveryDependentModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => (await context.GetModule<RecoveringModule>()).Value;
    }

    private class FailureReplacementModule : FreshReplacementModule
    {
        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
            => Task.FromResult<ModuleResult<string>?>(result.ToFailure(new InvalidOperationException("replacement failure")));
    }

    private class SkippedReplacementModule : FreshReplacementModule
    {
        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
            => Task.FromResult<ModuleResult<string>?>(new ModuleResult<string>.Skipped(SkipDecision.Skip("replacement skip"))
            {
                Name = "incorrect",
                Duration = TimeSpan.Zero,
                StartTime = default,
                EndTime = default,
                Status = ModuleStatus.Succeeded,
            });
    }

    private class IgnoredFailureReplacementModule : RecoveringModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithIgnoreFailures();

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
            => Task.FromResult<ModuleResult<string>?>(result.ToFailure(new InvalidOperationException("replacement failure")));
    }
}
