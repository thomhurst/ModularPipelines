using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Execution;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Reporting;
using ModularPipelines.TestHelpers;
using Moq;

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
    public async Task FailureReplacement_PreservesExecutionTimingAndExceptionStack()
    {
        await using var host = await TestPipelineBuilder.Create().AddModule<DelayedFailureReplacementModule>().BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<DelayedFailureReplacementModule>().Single();

        await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());
        var result = await module;
        var original = module.Original!;

        using (Assert.Multiple())
        {
            await Assert.That(result.StartTime).IsEqualTo(original.StartTime);
            await Assert.That(result.EndTime).IsEqualTo(original.EndTime);
            await Assert.That(result.Duration).IsEqualTo(original.Duration);
            await Assert.That(result.ExceptionOrDefault!.StackTrace!).Contains(nameof(ThrowReplacementFailure));
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RecoveryFinalizationFailure_InvokesFailureHookAndAppliesFailurePolicy(bool ignoreFailures)
    {
        RecoveryFinalizationModule module = ignoreFailures
            ? new IgnoredRecoveryFinalizationModule()
            : new RecoveryFinalizationModule();
        var executionContext = new ModuleExecutionContext<string>(module, module.GetType());
        var services = new Mock<IServicesContext>();
        services.SetupGet(x => x.Options).Returns(new PipelineOptions());
        var moduleContext = new Mock<IModuleContext>();
        moduleContext.SetupGet(x => x.Logger).Returns(Mock.Of<IInternalModuleLogger>());
        moduleContext.SetupGet(x => x.Services).Returns(services.Object);
        using var engineCancellation = new ModularPipelines.Engine.EngineCancellationToken(new PrimaryExceptionContainer());
        var conditions = new Mock<IModuleConditionHandler>();
        conditions.Setup(x => x.ShouldIgnore(module, It.IsAny<CancellationToken>())).ReturnsAsync((false, null));
        var pipeline = new ModuleExecutionPipeline(
            Mock.Of<IModuleResultRepository>(),
            engineCancellation,
            new DirectHookInvoker(NullLogger<DirectHookInvoker>.Instance),
            conditions.Object,
            Microsoft.Extensions.Options.Options.Create(new PipelineOptions()));
        var finalizationFailure = new InvalidOperationException("artifact upload failed");
        var finalizedStatuses = new List<ModuleStatus>();
        var execution = pipeline.ExecuteAsync(module, executionContext, moduleContext.Object, CancellationToken.None,
            finalizeExecutionAsync: (result, _) =>
            {
                finalizedStatuses.Add(result.Status);
                return result is ModuleResult<string>.Success
                    ? Task.FromException(finalizationFailure)
                    : Task.CompletedTask;
            });

        if (ignoreFailures)
        {
            await execution;
        }
        else
        {
            await Assert.ThrowsAsync<ModuleFailedException>(() => execution);
        }

        var result = await module;
        using (Assert.Multiple())
        {
            await Assert.That(module.Failures.Count).IsEqualTo(2);
            await Assert.That(module.Failures[0].Message).IsEqualTo("original failure");
            await Assert.That(module.Failures[1]).IsSameReferenceAs(finalizationFailure);
            await Assert.That(result.ExceptionOrDefault).IsSameReferenceAs(finalizationFailure);
            await Assert.That(result.Status).IsEqualTo(ignoreFailures ? ModuleStatus.FailureIgnored : ModuleStatus.Failed);
            await Assert.That(module.AfterHookCount).IsEqualTo(1);
            await Assert.That(finalizedStatuses).IsEquivalentTo(ignoreFailures
                ? new[] { ModuleStatus.Succeeded, ModuleStatus.FailureIgnored }
                : new[] { ModuleStatus.Succeeded });
        }
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task SkippedReplacement_InvokesSkipHookOnce_AndPreservesSkippedResult(bool executionFails, bool skipHookThrows)
    {
        var builder = TestPipelineBuilder.Create();
        if (executionFails)
        {
            builder.AddModule<FailureSkippedReplacementModule>();
        }
        else
        {
            builder.AddModule<SkippedReplacementModule>();
        }

        await using var host = await builder.BuildAsync();
        var module = host.Services.GetServices<IModule>().OfType<SkippedReplacementModule>().Single();
        module.SkipHookThrows = skipHookThrows;
        await host.RunAsync();
        var result = await module;

        await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
        await Assert.That(result.SkipDecisionOrDefault!.Reason).IsEqualTo("replacement skip");
        await Assert.That(module.SkipDecisions.Count).IsEqualTo(1);
        await Assert.That(module.SkipDecisions[0]).IsSameReferenceAs(result.SkipDecisionOrDefault);
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

    private class DelayedFailureReplacementModule : Module<string>
    {
        public ModuleResult<string>? Original { get; private set; }

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult("original");

        protected override async Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
        {
            Original = result;
            await Task.Delay(30, cancellationToken);
            try
            {
                ThrowReplacementFailure();
                return null;
            }
            catch (InvalidOperationException exception)
            {
                return result.ToFailure(exception);
            }
        }
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
    private static void ThrowReplacementFailure() => throw new InvalidOperationException("replacement failure");

    private class RecoveryFinalizationModule : RecoveringModule
    {
        public List<Exception> Failures { get; } = [];

        public int AfterHookCount { get; private set; }

        protected override Task OnFailedAsync(IModuleContext context, Exception exception, CancellationToken cancellationToken)
        {
            Failures.Add(exception);
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
        {
            AfterHookCount++;
            return base.OnAfterExecuteAsync(context, result, cancellationToken);
        }
    }

    private class IgnoredRecoveryFinalizationModule : RecoveryFinalizationModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithIgnoreFailures();
    }

    private class SkippedReplacementModule : FreshReplacementModule
    {
        public bool SkipHookThrows { get; set; }

        public List<SkipDecision> SkipDecisions { get; } = [];

        protected override Task OnSkippedAsync(IModuleContext context, SkipDecision skipDecision, CancellationToken cancellationToken)
        {
            SkipDecisions.Add(skipDecision);
            return SkipHookThrows
                ? Task.FromException(new InvalidOperationException("skip hook failed"))
                : Task.CompletedTask;
        }

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

    private class FailureSkippedReplacementModule : SkippedReplacementModule
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => throw new InvalidOperationException("original failure");
    }

    private class IgnoredFailureReplacementModule : RecoveringModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module.WithIgnoreFailures();

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context, ModuleResult<string> result, CancellationToken cancellationToken)
            => Task.FromResult<ModuleResult<string>?>(result.ToFailure(new InvalidOperationException("replacement failure")));
    }
}
