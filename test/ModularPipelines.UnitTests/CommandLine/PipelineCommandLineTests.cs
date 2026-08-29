using ModularPipelines.Logging;
using ModularPipelines.Reporting;
using ModularPipelines.Events;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Caching;
using ModularPipelines;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.PipelineCli;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.UnitTests.PipelineCli;

[TUnit.Core.NotInParallel(nameof(PipelineCommandLineTests))]
public class PipelineCommandLineTests
{
    private static int _dependencyExecutions;
    private static int _targetExecutions;
    private static int _unrelatedExecutions;
    private static int _categoryExecutions;
    private static int _conditionEvaluations;

    private sealed class CapturingConsoleWriter : IConsoleWriter
    {
        public IRenderable? Renderable { get; private set; }

        public List<string> Messages { get; } = [];

        public void LogToConsole(string value)
        {
            Messages.Add(value);
        }

        public void Write(IRenderable renderable)
        {
            Renderable = renderable;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class AddRegistrationDependencyAttribute(Type dependencyType)
        : Attribute, IPlanningSafeModuleRegistrationHandler
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.AddDependency(dependencyType);
            return Task.CompletedTask;
        }
    }

    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _dependencyExecutions);
            return Task.FromResult<string>("dependency");
        }
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    private sealed class TargetModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _targetExecutions);
            return Task.FromResult<string>("target");
        }
    }

    private sealed class UnrelatedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _unrelatedExecutions);
            return Task.FromResult<string>("unrelated");
        }
    }

    private sealed class ConfiguredCategoryModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCategory("configured-category");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult("configured-category");
    }

    private sealed class CacheCandidateModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCacheKeyPart("plan-v1");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    private sealed class UnregisteredModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string>("unregistered");
    }

    private sealed class ThrowingConstructorModule : Module<string>
    {
        public ThrowingConstructorModule() =>
            throw new InvalidOperationException("Help must not activate modules.");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Help must not execute modules.");
    }

    private sealed class ThrowingConfigurationModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) =>
            throw new InvalidOperationException("Help must not read module configuration.");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Help must not execute modules.");
    }

    [AddRegistrationDependency(typeof(UnregisteredModule))]
    [ModuleCategory("excluded")]
    private sealed class InvalidDynamicDependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string>("invalid");
    }

    [AddRegistrationDependency(typeof(DependencyModule))]
    private sealed class RegistrationDependentModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    private sealed class ResultDependentSkipModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(async (context, _) =>
            {
                await context.GetModule<DependencyModule>();
                return SkipDecision.Skip("dependency result matched");
            });

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    private sealed class UnknownThenSkippedModule : DryRunModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(async (context, _) =>
            {
                await context.GetModule<DependencyModule>();
                return SkipDecision.DoNotSkip;
            })
            .WithSkipWhen(_ => SkipDecision.Skip("later condition"));
    }

    [ModularPipelines.DependsOn<DependencyModule>]
    private sealed class UnknownAndDoNotSkipModule : DryRunModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhenAll(
                async (context, _) =>
                {
                    await context.GetModule<DependencyModule>();
                    return SkipDecision.Skip("result matched");
                },
                (_, _) => ValueTask.FromResult(SkipDecision.DoNotSkip));
    }

    [ModularPipelines.DependsOn<ResultDependentSkipModule>]
    private sealed class DependentOnUnknownModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModuleCategory("selected")]
    private sealed class SelectedCategoryModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _categoryExecutions);
            return Task.FromResult<string>("selected");
        }
    }

    private sealed class TrackingCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context)
        {
            Interlocked.Increment(ref _conditionEvaluations);
            return Task.FromResult(true);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class RuntimeOnlyDependencyAttribute : DependsOnBaseAttribute
    {
        public override bool ShouldDependOn(Type candidateModule, IDependencyContext context) =>
            throw new InvalidOperationException("Graph export must not evaluate runtime dependency predicates.");
    }

    [RuntimeOnlyDependency]
    private sealed class RuntimeOnlyDependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult("runtime-only-dependency");
    }

    [RunIfAll<TrackingCondition>]
    private sealed class ConditionalModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string>("conditional");
    }

    private sealed class NeverRunCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    [RunIfAll<NeverRunCondition>]
    [ModuleCategory("selected")]
    private sealed class AttributeSkippedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult("attribute-skipped");
    }

    private sealed class FluentlySkippedModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCategory("selected")
            .WithSkipWhen(_ => SkipDecision.Skip("fluent skip"));

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    private sealed class SkippedDependencyModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCategory("selected")
            .WithSkipWhen(_ => SkipDecision.Skip("dependency unavailable"));

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModuleCategory("excluded")]
    private sealed class ExcludedSkippedDependencyModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("dependency unavailable"));

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModularPipelines.DependsOn<SkippedDependencyModule>]
    private sealed class DependentOnSkippedModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithCategory("selected");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModularPipelines.DependsOn<SkippedDependencyModule>]
    private sealed class ResultDependentOnSkippedModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(async (context, _) =>
            {
                await context.GetModule<SkippedDependencyModule>();
                return SkipDecision.DoNotSkip;
            });

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    private sealed class PlanEstimatedTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType) => Task.FromResult(moduleType.Name switch
        {
            nameof(DependencyModule) => TimeSpan.FromMinutes(2),
            nameof(UnrelatedModule) => TimeSpan.FromMinutes(5),
            nameof(TargetModule) => TimeSpan.FromMinutes(3),
            nameof(FirstLockedModule) => TimeSpan.FromMinutes(2),
            nameof(SecondLockedModule) => TimeSpan.FromMinutes(3),
            nameof(FirstParallelRootModule) => TimeSpan.FromMinutes(10),
            nameof(SecondParallelRootModule) => TimeSpan.FromMinutes(10),
            nameof(ThirdParallelRootModule) => TimeSpan.FromMinutes(10),
            nameof(ParallelJoinModule) => TimeSpan.FromMinutes(10),
            nameof(AlphabeticallyFirstLowPriorityModule) => TimeSpan.FromMinutes(100),
            nameof(FirstHighPriorityModule) => TimeSpan.FromMinutes(1),
            nameof(SecondHighPriorityModule) => TimeSpan.FromMinutes(1),
            nameof(FirstCpuModule) => TimeSpan.FromMinutes(10),
            nameof(SecondCpuModule) => TimeSpan.FromMinutes(10),
            nameof(FirstIoModule) => TimeSpan.FromMinutes(10),
            nameof(SecondIoModule) => TimeSpan.FromMinutes(10),
            nameof(FirstLimitedModule) => TimeSpan.FromMinutes(10),
            nameof(SecondLimitedModule) => TimeSpan.FromMinutes(10),
            nameof(IndependentRootModule) => TimeSpan.FromMinutes(10),
            nameof(LongTailModule) => TimeSpan.FromMinutes(100),
            nameof(CpuHolderModule) => TimeSpan.FromMinutes(10),
            nameof(CpuAndLimiterWaiterModule) => TimeSpan.FromMinutes(10),
            nameof(LimiterOnlyModule) => TimeSpan.FromMinutes(10),
            nameof(PriorityConstraintRootModule) => TimeSpan.FromMinutes(1),
            nameof(PriorityConstraintDependentModule) => TimeSpan.FromMinutes(1),
            nameof(PriorityConstraintLowRootModule) => TimeSpan.FromMinutes(10),
            nameof(PriorityConstraintTailModule) => TimeSpan.FromMinutes(100),
            nameof(QueuedCriticalRootModule) => TimeSpan.FromMinutes(1),
            nameof(QueuedHighRootModule) => TimeSpan.FromMinutes(100),
            nameof(QueuedLowRootModule) => TimeSpan.FromMinutes(100),
            nameof(QueuedCriticalChildModule) => TimeSpan.FromMinutes(1),
            nameof(QueuedLongTailModule) => TimeSpan.FromMinutes(1000),
            _ => TimeSpan.Zero,
        });

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration) => Task.CompletedTask;

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;
    }

    private sealed class PlanHistoryRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) => Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            if (module is not SkippedDependencyModule and not ExcludedSkippedDependencyModule)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    private sealed class FirstLockedModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithNotInParallel("shared-plan-lock");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    private sealed class SecondLockedModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithNotInParallel("shared-plan-lock");

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    private sealed class FirstParallelRootModule : DryRunModule
    {
    }

    private sealed class SecondParallelRootModule : DryRunModule
    {
    }

    private sealed class ThirdParallelRootModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<FirstParallelRootModule>]
    [ModularPipelines.DependsOn<SecondParallelRootModule>]
    [ModularPipelines.DependsOn<ThirdParallelRootModule>]
    private sealed class ParallelJoinModule : DryRunModule
    {
    }

    [Priority(ModulePriority.Low)]
    private sealed class AlphabeticallyFirstLowPriorityModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    private sealed class FirstHighPriorityModule : DryRunModule
    {
    }

    private sealed class SecondHighPriorityModule : DryRunModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithPriority(ModulePriority.High);
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    private sealed class FirstCpuModule : DryRunModule
    {
    }

    [ExecutionHint(ExecutionHint.CpuBound)]
    private sealed class SecondCpuModule : DryRunModule
    {
    }

    [ExecutionHint(ExecutionHint.IoBound)]
    private sealed class FirstIoModule : DryRunModule
    {
    }

    [ExecutionHint(ExecutionHint.IoBound)]
    private sealed class SecondIoModule : DryRunModule
    {
    }

    private sealed class SingleModuleLimit : IParallelLimit
    {
        public static int Limit => 1;
    }

    private sealed class InvalidModuleLimit : IParallelLimit
    {
        public static int Limit => 0;
    }

    [ModularPipelines.Attributes.ParallelLimiter<InvalidModuleLimit>]
    private sealed class ExcludedInvalidLimitModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    [ModularPipelines.Attributes.ParallelLimiter<SingleModuleLimit>]
    private sealed class FirstLimitedModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    [ModularPipelines.Attributes.ParallelLimiter<SingleModuleLimit>]
    private sealed class SecondLimitedModule : DryRunModule
    {
    }

    private sealed class IndependentRootModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<IndependentRootModule>]
    private sealed class LongTailModule : DryRunModule
    {
    }

    [Priority(ModulePriority.Critical)]
    [ExecutionHint(ExecutionHint.CpuBound)]
    private sealed class CpuHolderModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    [ExecutionHint(ExecutionHint.CpuBound)]
    [ModularPipelines.Attributes.ParallelLimiter<SingleModuleLimit>]
    private sealed class CpuAndLimiterWaiterModule : DryRunModule
    {
    }

    [ModularPipelines.Attributes.ParallelLimiter<SingleModuleLimit>]
    private sealed class LimiterOnlyModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    [ModularPipelines.Attributes.NotInParallel("priority-plan-lock")]
    private sealed class PriorityConstraintRootModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    [ModularPipelines.Attributes.NotInParallel("priority-plan-lock")]
    [ModularPipelines.DependsOn<PriorityConstraintRootModule>]
    private sealed class PriorityConstraintDependentModule : DryRunModule
    {
    }

    [Priority(ModulePriority.Low)]
    [ModularPipelines.Attributes.NotInParallel("priority-plan-lock")]
    private sealed class PriorityConstraintLowRootModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<PriorityConstraintDependentModule>]
    private sealed class PriorityConstraintTailModule : DryRunModule
    {
    }

    [Priority(ModulePriority.Critical)]
    private sealed class QueuedCriticalRootModule : DryRunModule
    {
    }

    [Priority(ModulePriority.High)]
    private sealed class QueuedHighRootModule : DryRunModule
    {
    }

    [Priority(ModulePriority.Low)]
    private sealed class QueuedLowRootModule : DryRunModule
    {
    }

    [Priority(ModulePriority.Critical)]
    [ModularPipelines.DependsOn<QueuedCriticalRootModule>]
    private sealed class QueuedCriticalChildModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<QueuedCriticalChildModule>]
    private sealed class QueuedLongTailModule : DryRunModule
    {
    }

    private sealed class MissingOptionalModule : DryRunModule
    {
    }

    private sealed class RegistrationOnlyOptionalSkipModule : DryRunModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(context => context.GetModuleIfRegistered<MissingOptionalModule>() is null
                ? SkipDecision.Skip("optional module absent")
                : SkipDecision.DoNotSkip);
    }

    private sealed class OptionalResultDependentSkipModule : DryRunModule
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(async (context, _) =>
            {
                var dependency = context.GetModuleIfRegistered<DependencyModule>();
                if (dependency is null)
                {
                    return SkipDecision.Skip("optional module absent");
                }

                await dependency;
                return SkipDecision.DoNotSkip;
            });
    }

    [ModularPipelines.DependsOn<SkippedCycleBModule>]
    [ModuleCategory("excluded")]
    private sealed class SkippedCycleAModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<SkippedCycleAModule>]
    [ModuleCategory("excluded")]
    private sealed class SkippedCycleBModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<ExcludedSkippedDependencyModule>]
    [ModularPipelines.DependsOn<HistoryCycleBModule>]
    [ModuleCategory("selected")]
    private sealed class HistoryCycleAModule : DryRunModule
    {
    }

    [ModularPipelines.DependsOn<ExcludedSkippedDependencyModule>]
    [ModularPipelines.DependsOn<HistoryCycleAModule>]
    [ModuleCategory("selected")]
    private sealed class HistoryCycleBModule : DryRunModule
    {
    }

    private abstract class DryRunModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [Before(Test)]
    public void ResetCounters()
    {
        _dependencyExecutions = 0;
        _targetExecutions = 0;
        _unrelatedExecutions = 0;
        _categoryExecutions = 0;
        _conditionEvaluations = 0;
    }

    [Test]
    public async Task ModuleOptionRunsTargetAndDependencyClosure()
    {
        var builder = CreateExecutionBuilder(["--module", nameof(TargetModule)]);

        await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(_dependencyExecutions).IsEqualTo(1);
            await Assert.That(_targetExecutions).IsEqualTo(1);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task ProgrammaticTargetModulesRunsDependencyClosure()
    {
        var builder = CreateExecutionBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            TargetModules = [nameof(TargetModule)],
        });

        await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(_dependencyExecutions).IsEqualTo(1);
            await Assert.That(_targetExecutions).IsEqualTo(1);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task SkipModuleOptionExcludesNamedModule()
    {
        var builder = CreateExecutionBuilder(["--skip-module", nameof(UnrelatedModule)]);

        await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(_dependencyExecutions).IsEqualTo(1);
            await Assert.That(_targetExecutions).IsEqualTo(1);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task CategoriesOptionBindsExistingPipelineOption()
    {
        var builder = Pipeline.CreateBuilder(["--categories", "selected"]);
        builder.AddModule<SelectedCategoryModule>();
        builder.AddModule<UnrelatedModule>();

        await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.RunOnlyCategories).IsEquivalentTo(["selected"]);
            await Assert.That(_categoryExecutions).IsEqualTo(1);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task NoCacheOptionBindsPipelineOption()
    {
        var builder = Pipeline.CreateBuilder(["--no-cache"]);

        await Assert.That(builder.Options.DisableModuleCache).IsTrue();
        await Assert.That(builder.Configuration["no-cache"]).IsNull();
    }

    [Test]
    public async Task UnknownArgumentsStillFlowToHostConfiguration()
    {
        var builder = Pipeline.CreateBuilder(
        [
            "--module", nameof(TargetModule),
            "--custom-setting", "custom-value",
        ]);

        await Assert.That(builder.Options.TargetModules).IsEquivalentTo([nameof(TargetModule)]);
        await Assert.That(builder.Configuration["custom-setting"]).IsEqualTo("custom-value");
    }

    [Test]
    public async Task ShortHostOptionIsNotMistakenForPipelineTypo()
    {
        var builder = Pipeline.CreateBuilder(["--mode", "safe"]);

        await Assert.That(builder.Configuration["mode"]).IsEqualTo("safe");
    }

    [Test]
    public async Task SeparatorForwardsLikelyTypoToHostConfiguration()
    {
        var builder = Pipeline.CreateBuilder(["--", "--skip-modules", "Deploy"]);

        await Assert.That(builder.Options.SkippedModules).IsNull();
        await Assert.That(builder.Configuration["skip-modules"]).IsEqualTo("Deploy");
    }

    [Test]
    [Arguments("--skip-modules", "--skip-module")]
    [Arguments("--dryrun", "--dry-run")]
    [Arguments("--ignore-category", "--ignore-categories")]
    public async Task LikelyPipelineOptionTypoFailsWithSuggestion(string typo, string suggestion)
    {
        var exception = await Assert.That(() => Pipeline.CreateBuilder([typo]))
            .Throws<ArgumentException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains($"Did you mean '{suggestion}'?");
            await Assert.That(exception.Message).Contains("ModularPipelines command-line options:");
            await Assert.That(exception.Message).Contains("Use '--'");
        }
    }

    [Test]
    public async Task MissingOptionValueIncludesUsage()
    {
        var exception = await Assert.That(() => Pipeline.CreateBuilder(["--module"]))
            .Throws<ArgumentException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("requires a value");
            await Assert.That(exception.Message).Contains("ModularPipelines command-line options:");
        }
    }

    [Test]
    [Arguments("--help=value", "--help")]
    [Arguments("--list-modules=value", "--list-modules")]
    [Arguments("--validate=value", "--validate")]
    [Arguments("--dry-run=value", "--dry-run")]
    public async Task FlagOptionValueFailsWithClearError(string argument, string option)
    {
        var exception = await Assert.That(() => Pipeline.CreateBuilder([argument]))
            .Throws<ArgumentException>();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message)
                .Contains($"Command-line option '{option}' does not accept a value.");
            await Assert.That(exception.Message).DoesNotContain("Did you mean");
            await Assert.That(exception.Message).Contains("ModularPipelines command-line options:");
        }
    }

    [Test]
    public async Task RepeatedCommaSeparatedAndEqualsValuesAreParsed()
    {
        var builder = Pipeline.CreateBuilder(
        [
            "--module", "FirstModule,SecondModule",
            "--module=ThirdModule",
            "--skip-module=SkippedModule",
            "--categories", "Build,Test",
            "--ignore-categories=Integration",
        ]);

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.TargetModules)
                .IsEquivalentTo(["FirstModule", "SecondModule", "ThirdModule"]);
            await Assert.That(builder.Options.SkippedModules)
                .IsEquivalentTo(["SkippedModule"]);
            await Assert.That(builder.Options.RunOnlyCategories)
                .IsEquivalentTo(["Build", "Test"]);
            await Assert.That(builder.Options.IgnoreCategories)
                .IsEquivalentTo(["Integration"]);
        }
    }

    [Test]
    public async Task AssemblyQualifiedModuleNameIsPreserved()
    {
        var builder = CreateExecutionBuilder(
            ["--module", typeof(TargetModule).AssemblyQualifiedName!]);

        await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(_dependencyExecutions).IsEqualTo(1);
            await Assert.That(_targetExecutions).IsEqualTo(1);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task SelectionValidationDoesNotEvaluateRunConditions()
    {
        var builder = Pipeline.CreateBuilder(["--module", nameof(ConditionalModule)]);
        builder.AddModule<ConditionalModule>();

        var result = await builder.ValidateAsync();

        using (Assert.Multiple())
        {
            await Assert.That(result.IsValid).IsTrue();
            await Assert.That(_conditionEvaluations).IsEqualTo(0);
        }
    }

    [Test]
    public async Task CommandLineParsingCanBeDisabled()
    {
        var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
        {
            Args = ["--module", "forwarded-value"],
            EnableCommandLineOptions = false,
        });

        await Assert.That(builder.Options.TargetModules).IsNull();
        await Assert.That(builder.Configuration["module"]).IsEqualTo("forwarded-value");
    }

    [Test]
    public async Task UnknownTargetProducesValidationError()
    {
        var builder = CreateExecutionBuilder(["--module", "MissingModule"]);

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(
            () => builder.RunAsync());

        await Assert.That(exception!.ValidationResult.Errors.Any(
            error => error.Message.Contains("MissingModule", StringComparison.Ordinal))).IsTrue();
    }

    [Test]
    [Arguments("--help")]
    [Arguments("-h")]
    [Arguments("--list-modules")]
    [Arguments("--validate")]
    public async Task InformationalCommandsDoNotExecuteModules(string command)
    {
        var builder = CreateExecutionBuilder([command]);

        var summary = await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
            await Assert.That(_targetExecutions).IsEqualTo(0);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task GraphCommandExportsWithoutExecutingModules()
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-cli-graph-");
        try
        {
            var path = Path.Combine(directory.FullName, "pipeline.json");
            var builder = CreateExecutionBuilder(["--graph", "json", path]);

            var summary = await builder.RunAsync();
            var graph = await File.ReadAllTextAsync(path);

            using (Assert.Multiple())
            {
                await Assert.That(summary.Results).IsEmpty();
                await Assert.That(graph).Contains(nameof(DependencyModule));
                await Assert.That(graph).Contains(nameof(TargetModule));
                await Assert.That(_dependencyExecutions).IsEqualTo(0);
                await Assert.That(_targetExecutions).IsEqualTo(0);
                await Assert.That(_unrelatedExecutions).IsEqualTo(0);
            }
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task GraphCommandDoesNotEvaluateRuntimeDependencyPredicatesDuringBuild()
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-cli-graph-");
        try
        {
            var path = Path.Combine(directory.FullName, "pipeline.json");
            var builder = Pipeline.CreateBuilder(["--graph", "json", path]);
            builder.AddModule<DependencyModule>();
            builder.AddModule<RuntimeOnlyDependencyModule>();

            var summary = await builder.RunAsync();
            var graph = await File.ReadAllTextAsync(path);

            using (Assert.Multiple())
            {
                await Assert.That(summary.Results).IsEmpty();
                await Assert.That(graph).Contains(nameof(RuntimeOnlyDependencyModule));
            }
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments("--help")]
    [Arguments("-h")]
    public async Task HelpOptionsPrintUsage(string option)
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = CreateExecutionBuilder([option]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);

        await builder.RunAsync();

        await Assert.That(consoleWriter.Messages.Single())
            .Contains("ModularPipelines command-line options:")
            .And.Contains("--skip-module")
            .And.Contains("--");
        await Assert.That(PipelineCommandLineHelp.Usage)
            .Contains("--graph <mermaid|dot|json> [path]")
            .And.Contains("--graph-path <path>");
    }

    [Test]
    public async Task HelpDoesNotRequireAValidPipeline()
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = Pipeline.CreateBuilder(["--help"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);

        var summary = await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(consoleWriter.Messages.Single())
                .Contains("ModularPipelines command-line options:");
        }
    }

    [Test]
    public async Task HelpDoesNotActivateModules()
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = Pipeline.CreateBuilder(["--help"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);
        builder.AddModule<ThrowingConstructorModule>();

        var summary = await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(consoleWriter.Messages.Single())
                .Contains("ModularPipelines command-line options:");
        }
    }

    [Test]
    public async Task HelpDoesNotReadModuleConfiguration()
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = Pipeline.CreateBuilder(["--help"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);
        builder.AddModule<ThrowingConfigurationModule>();

        var summary = await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(consoleWriter.Messages.Single())
                .Contains("ModularPipelines command-line options:");
        }
    }

    [Test]
    public async Task PlanAsyncBuildsDependencyOrderedWavesWithoutExecutingModules()
    {
        var builder = CreateExecutionBuilder();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        using (Assert.Multiple())
        {
            await Assert.That(plan.Waves).Count().IsEqualTo(2);
            await Assert.That(plan.Waves[0].Modules.Select(module => module.Module.GetType()))
                .IsEquivalentTo([typeof(DependencyModule), typeof(UnrelatedModule)]);
            await Assert.That(plan.Waves[1].Modules.Select(module => module.Module.GetType()))
                .IsEquivalentTo([typeof(TargetModule)]);
            await Assert.That(plan.Waves[0].EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(5));
            await Assert.That(plan.Waves[1].EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(3));
            await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(5));
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
            await Assert.That(_targetExecutions).IsEqualTo(0);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PlanAsyncSerializesModulesSharingNotInParallelKeyInEstimate()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<FirstLockedModule>();
        builder.AddModule<SecondLockedModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(5));
    }

    [Test]
    public async Task PlanAsyncSimulatesMaxParallelismSlotsInEstimate()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });
        builder.AddModule<FirstParallelRootModule>();
        builder.AddModule<SecondParallelRootModule>();
        builder.AddModule<ThirdParallelRootModule>();
        builder.AddModule<ParallelJoinModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(30));
    }

    [Test]
    public async Task PlanAsyncSchedulesReadyModulesByPriorityInEstimate()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });
        builder.AddModule<AlphabeticallyFirstLowPriorityModule>();
        builder.AddModule<FirstHighPriorityModule>();
        builder.AddModule<SecondHighPriorityModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(101));
    }

    [Test]
    public async Task PlanAsyncAppliesExecutionHintLimitsInEstimate()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with
            {
                MaxParallelism = 10,
                MaxCpuIntensiveModules = 1,
                MaxIoIntensiveModules = 1,
            },
        });
        builder.AddModule<FirstCpuModule>();
        builder.AddModule<SecondCpuModule>();
        builder.AddModule<FirstIoModule>();
        builder.AddModule<SecondIoModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(20));
    }

    [Test]
    public async Task PlanAsyncAppliesCustomParallelLimitersInEstimate()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<FirstLimitedModule>();
        builder.AddModule<SecondLimitedModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(20));
    }

    [Test]
    public async Task PlanAsyncCountsParallelLimiterWaitersAgainstWorkerSlots()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });
        builder.AddModule<FirstLimitedModule>();
        builder.AddModule<SecondLimitedModule>();
        builder.AddModule<IndependentRootModule>();
        builder.AddModule<LongTailModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(120));
    }

    [Test]
    public async Task PlanAsyncModelsLimiterAcquisitionBeforeExecutionHintWait()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with
            {
                MaxParallelism = 3,
                MaxCpuIntensiveModules = 1,
            },
        });
        builder.AddModule<CpuHolderModule>();
        builder.AddModule<CpuAndLimiterWaiterModule>();
        builder.AddModule<LimiterOnlyModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(30));
    }

    [Test]
    public async Task PlanAsyncReprioritizesModulesWhenConstraintReleases()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });
        builder.AddModule<PriorityConstraintRootModule>();
        builder.AddModule<PriorityConstraintDependentModule>();
        builder.AddModule<PriorityConstraintLowRootModule>();
        builder.AddModule<PriorityConstraintTailModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(102));
    }

    [Test]
    public async Task PlanAsyncPreservesReadyChannelQueueOrder()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            Concurrency = options.Concurrency with { MaxParallelism = 2 },
        });
        builder.AddModule<QueuedCriticalRootModule>();
        builder.AddModule<QueuedHighRootModule>();
        builder.AddModule<QueuedLowRootModule>();
        builder.AddModule<QueuedCriticalChildModule>();
        builder.AddModule<QueuedLongTailModule>();
        builder.AddModuleEstimatedTimeProvider<PlanEstimatedTimeProvider>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(1101));
    }

    [Test]
    public async Task PlanAsyncDefersOptionalRegistrationChecks()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<RegistrationOnlyOptionalSkipModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is RegistrationOnlyOptionalSkipModule);

        using (Assert.Multiple())
        {
            await Assert.That(plannedModule.IsSkipDecisionKnown).IsFalse();
            await Assert.That(plannedModule.ShouldSkip).IsFalse();
        }
    }

    [Test]
    public async Task MermaidGraphCommandDefaultsToRawMermaidExtension()
    {
        var options = PipelineCommandLineParser.Parse(["--graph", "mermaid"]);

        await Assert.That(options.GraphPath).IsEqualTo("dependency-graph.mmd");
    }

    [Test]
    public async Task GraphCommandDoesNotConsumeHostConfigurationAsPath()
    {
        var options = PipelineCommandLineParser.Parse(["--graph", "json", "Feature:Enabled=true"]);

        using (Assert.Multiple())
        {
            await Assert.That(options.GraphPath).IsEqualTo("dependency-graph.json");
            await Assert.That(options.HostArguments).IsEquivalentTo(["Feature:Enabled=true"]);
        }
    }

    [Test]
    public async Task GraphCommandDoesNotConsumeShortHelpAsPath()
    {
        var exception = await Assert.That(() =>
                PipelineCommandLineParser.Parse(["--graph", "json", "-h"]))
            .Throws<ArgumentException>();

        await Assert.That(exception!.Message)
            .Contains("'-h' cannot be combined with another pipeline command");
    }

    [Test]
    public async Task GraphCommandAcceptsPathContainingEqualsSign()
    {
        var options = PipelineCommandLineParser.Parse(
            ["--graph", "json", "artifacts/branch=main.json"]);

        await Assert.That(options.GraphPath)
            .IsEqualTo("artifacts/branch=main.json");
    }

    [Test]
    public async Task GraphCommandAcceptsExplicitAmbiguousPath()
    {
        var options = PipelineCommandLineParser.Parse(
            ["--graph", "json", "--graph-path", "branch=main.json"]);

        await Assert.That(options.GraphPath).IsEqualTo("branch=main.json");
    }

    [Test]
    public async Task GraphPathOptionRequiresGraphCommand()
    {
        await Assert.That(() => PipelineCommandLineParser.Parse(
                ["--graph-path", "branch=main.json"]))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task GraphCommandRejectsDuplicatePaths()
    {
        await Assert.That(() => PipelineCommandLineParser.Parse(
                ["--graph", "json", "graph.json", "--graph-path", "other.json"]))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task GraphCommandRejectsRepeatedFormats()
    {
        await Assert.That(() => PipelineCommandLineParser.Parse(
                ["--graph", "json", "report.json", "--graph", "dot"]))
            .Throws<ArgumentException>();
    }

    [Test]
    [Arguments("0")]
    [Arguments("1")]
    [Arguments("2")]
    public async Task GraphCommandRejectsNumericFormats(string format)
    {
        await Assert.That(() => Pipeline.CreateBuilder(["--graph", format]))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task GraphCommandRejectsCombinedFormats()
    {
        await Assert.That(() => Pipeline.CreateBuilder(["--graph", "mermaid,dot"]))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task PlanAsyncMarksAwaitedOptionalResultUnknown()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<OptionalResultDependentSkipModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync().WaitAsync(TimeSpan.FromSeconds(5));
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is OptionalResultDependentSkipModule);

        await Assert.That(plannedModule.IsSkipDecisionKnown).IsFalse();
    }

    [Test]
    public async Task PlanAsyncKeepsExcludedDependencyCyclesVisible()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["selected"],
        });
        builder.AddModule<SkippedCycleAModule>();
        builder.AddModule<SkippedCycleBModule>();
        builder.AddModule<SelectedCategoryModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves
            .SelectMany(wave => wave.Modules)
            .ToDictionary(module => module.Module.GetType());

        using (Assert.Multiple())
        {
            await Assert.That(modules).ContainsKey(typeof(SkippedCycleAModule));
            await Assert.That(modules).ContainsKey(typeof(SkippedCycleBModule));
            await Assert.That(modules[typeof(SkippedCycleAModule)].ShouldSkip).IsTrue();
            await Assert.That(modules[typeof(SkippedCycleBModule)].ShouldSkip).IsTrue();
        }
    }

    [Test]
    public async Task PlanAsyncUsesRunnableSubsetWhenExcludedModuleHasInvalidDependency()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["selected"],
        });
        builder.AddModule<InvalidDynamicDependencyModule>();
        builder.AddModule<SelectedCategoryModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var invalidModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is InvalidDynamicDependencyModule);

        await Assert.That(invalidModule.ShouldSkip).IsTrue();
    }

    [Test]
    public async Task PlanAsyncDoesNotCascadeSkipWhenDependencyHistoryExists()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SkippedDependencyModule>();
        builder.AddModule<DependentOnSkippedModule>();
        builder.AddResultsRepository<PlanHistoryRepository>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var dependent = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is DependentOnSkippedModule);

        using (Assert.Multiple())
        {
            await Assert.That(dependent.IsSkipDecisionKnown).IsTrue();
            await Assert.That(dependent.ShouldSkip).IsFalse();
        }
    }

    [Test]
    public async Task PlanAsyncRevalidatesHistoryAwareRunnableGraph()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["selected"],
        });
        builder.AddModule<ExcludedSkippedDependencyModule>();
        builder.AddModule<HistoryCycleAModule>();
        builder.AddModule<HistoryCycleBModule>();
        builder.AddResultsRepository<PlanHistoryRepository>();
        await using var pipeline = await builder.BuildAsync();

        await Assert.ThrowsAsync<DependencyCollisionException>(() => pipeline.PlanAsync());
    }

    [Test]
    public async Task PlanAsyncIncludesRegistrationTimeDependenciesInWaves()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<RegistrationDependentModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();

        using (Assert.Multiple())
        {
            await Assert.That(plan.Waves).Count().IsEqualTo(2);
            await Assert.That(plan.Waves[0].Modules.Single().Module).IsTypeOf<DependencyModule>();
            await Assert.That(plan.Waves[1].Modules.Single().Module).IsTypeOf<RegistrationDependentModule>();
        }
    }

    [Test]
    public async Task PlanAsyncMarksResultDependentFluentSkipDecisionUnknown()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<ResultDependentSkipModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync().WaitAsync(TimeSpan.FromSeconds(5));
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is ResultDependentSkipModule);

        using (Assert.Multiple())
        {
            await Assert.That(plannedModule.IsSkipDecisionKnown).IsFalse();
            await Assert.That(plannedModule.ShouldSkip).IsFalse();
            await Assert.That((object?) plannedModule.SkipDecision).IsNull();
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PlanAsyncContinuesOrConditionsAfterUnknownDecision()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<UnknownThenSkippedModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is UnknownThenSkippedModule);

        using (Assert.Multiple())
        {
            await Assert.That(plannedModule.IsSkipDecisionKnown).IsTrue();
            await Assert.That(plannedModule.ShouldSkip).IsTrue();
            await Assert.That(plannedModule.SkipDecision!.Reason).IsEqualTo("later condition");
        }
    }

    [Test]
    public async Task PlanAsyncContinuesAndConditionsAfterUnknownDecision()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<UnknownAndDoNotSkipModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is UnknownAndDoNotSkipModule);

        using (Assert.Multiple())
        {
            await Assert.That(plannedModule.IsSkipDecisionKnown).IsTrue();
            await Assert.That(plannedModule.ShouldSkip).IsFalse();
        }
    }

    [Test]
    public async Task PlanAsyncPropagatesUnknownDecisionsToRequiredDependents()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<ResultDependentSkipModule>();
        builder.AddModule<DependentOnUnknownModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is DependentOnUnknownModule);

        await Assert.That(plannedModule.IsSkipDecisionKnown).IsFalse();
    }

    [Test]
    public async Task PlanAsyncPrefersCascadeSkipOverUnknownDecision()
    {
        var builder = Pipeline.CreateBuilder();
        builder.AddModule<SkippedDependencyModule>();
        builder.AddModule<ResultDependentOnSkippedModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var plannedModule = plan.Waves
            .SelectMany(wave => wave.Modules)
            .Single(module => module.Module is ResultDependentOnSkippedModule);

        using (Assert.Multiple())
        {
            await Assert.That(plannedModule.IsSkipDecisionKnown).IsTrue();
            await Assert.That(plannedModule.ShouldSkip).IsTrue();
            await Assert.That(plannedModule.SkipDecision!.Reason)
                .Contains(nameof(SkippedDependencyModule));
        }
    }

    [Test]
    public async Task PlanAsyncEvaluatesAllSkipSourcesAndCascadesDependencies()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["selected"],
        });
        builder.AddModule<FluentlySkippedModule>();
        builder.AddModule<AttributeSkippedModule>();
        builder.AddModule<SkippedDependencyModule>();
        builder.AddModule<DependentOnSkippedModule>();
        builder.AddModule<UnrelatedModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        using (Assert.Multiple())
        {
            await Assert.That(modules[typeof(FluentlySkippedModule)].SkipDecision!.Reason)
                .IsEqualTo("fluent skip");
            await Assert.That(modules[typeof(AttributeSkippedModule)].SkipDecision!.Reason)
                .Contains("RunIfAll");
            await Assert.That(modules[typeof(SkippedDependencyModule)].SkipDecision!.Reason)
                .IsEqualTo("dependency unavailable");
            await Assert.That(modules[typeof(DependentOnSkippedModule)].SkipDecision!.Reason)
                .Contains(nameof(SkippedDependencyModule));
            await Assert.That(modules[typeof(UnrelatedModule)].SkipDecision!.Reason)
                .Contains("runnable category");
            await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.Zero);
        }
    }

    [Test]
    public async Task PlanAsyncDoesNotValidateLimiterForExcludedModule()
    {
        var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(ExcludedInvalidLimitModule)],
        });
        builder.AddModule<ExcludedInvalidLimitModule>();
        await using var pipeline = await builder.BuildAsync();

        var plan = await pipeline.PlanAsync();
        var plannedModule = plan.Waves.SelectMany(wave => wave.Modules).Single();

        using (Assert.Multiple())
        {
            await Assert.That(plannedModule.ShouldSkip).IsTrue();
            await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.Zero);
        }
    }

    [Test]
    public async Task DryRunOptionPrintsPlanAndDoesNotExecuteModules()
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = CreateExecutionBuilder(["--dry-run"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);

        var summary = await builder.RunAsync();
        var output = Render(consoleWriter.Renderable!);

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.DryRun).IsTrue();
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(summary.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
            await Assert.That(consoleWriter.Renderable).IsNotNull();
            await Assert.That(output).Contains("Pipeline dry-run plan");
            await Assert.That(output).Contains("Wave ETA");
            await Assert.That(output).Contains(nameof(TargetModule));
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
            await Assert.That(_targetExecutions).IsEqualTo(0);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task DryRunMarksCacheCandidatesAndQualifiesEstimate()
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = Pipeline.CreateBuilder(["--dry-run"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);
        builder.AddModuleCache<FileSystemModuleCache>();
        builder.AddModule<CacheCandidateModule>();

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var summary = await pipeline.RunAsync();
        var output = Render(consoleWriter.Renderable!);

        using (Assert.Multiple())
        {
            await Assert.That(plan.Waves.SelectMany(wave => wave.Modules).Single().IsCacheCandidate)
                .IsTrue();
            await Assert.That(summary.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
            await Assert.That(output).Contains("Run (cache candidate)");
            await Assert.That(output).Contains("cache hits may reduce actual duration");
        }
    }

    [Test]
    public async Task DryRunDoesNotMarkCacheCandidatesWhenCacheIsDisabled()
    {
        var builder = Pipeline.CreateBuilder(["--dry-run", "--no-cache"]);
        builder.AddModuleCache<FileSystemModuleCache>();
        builder.AddModule<CacheCandidateModule>();

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();

        await Assert.That(plan.Waves.SelectMany(wave => wave.Modules).Single().IsCacheCandidate)
            .IsFalse();
    }

    [Test]
    public async Task ProgrammaticDryRunOptionDoesNotExecuteModules()
    {
        var builder = CreateExecutionBuilder();
        builder.ConfigurePipelineOptions(options => options with { DryRun = true });

        var summary = await builder.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
            await Assert.That(_targetExecutions).IsEqualTo(0);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task ValidateCommandChecksRegistrationTimeDependencies()
    {
        var builder = Pipeline.CreateBuilder(["--validate"]);
        builder.AddModule<InvalidDynamicDependencyModule>();

        await Assert.ThrowsAsync<ModuleNotRegisteredException>(
            () => builder.RunAsync());
    }

    [Test]
    public async Task ListModulesUsesFinalizedFluentCategory()
    {
        var consoleWriter = new CapturingConsoleWriter();
        var builder = Pipeline.CreateBuilder(["--list-modules"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);
        builder.AddModule<ConfiguredCategoryModule>();

        var summary = await builder.RunAsync();

        var output = Render(consoleWriter.Renderable!);

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("configured-category");
            await Assert.That(summary.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
        }
    }

    [Test]
    public async Task ValidateCommandReturnsSuccessfulSummary()
    {
        var builder = CreateExecutionBuilder(["--validate"]);

        var summary = await builder.RunAsync();

        await Assert.That(summary.Status).IsEqualTo(ModularPipelines.ModuleStatus.Succeeded);
    }

    private static string Render(IRenderable renderable)
    {
        using var output = new StringWriter();
        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
            Out = new AnsiConsoleOutput(output),
        });
        console.Profile.Width = 200;
        console.Write(renderable);
        return output.ToString();
    }

    [Test]
    public async Task PipelineCommandsAreMutuallyExclusive()
    {
        await Assert.That(() => Pipeline.CreateBuilder(["--list-modules", "--validate"]))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task DryRunCannotBeCombinedWithAnotherPipelineCommand()
    {
        await Assert.That(() => Pipeline.CreateBuilder(["--dry-run", "--validate"]))
            .Throws<ArgumentException>();
    }

    private static PipelineBuilder CreateExecutionBuilder(string[]? arguments = null)
    {
        var builder = Pipeline.CreateBuilder(arguments);
        builder.AddModule<DependencyModule>();
        builder.AddModule<TargetModule>();
        builder.AddModule<UnrelatedModule>();
        return builder;
    }
}
