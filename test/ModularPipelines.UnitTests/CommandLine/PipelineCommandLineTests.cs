using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Conditions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
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

        public void LogToConsole(string value)
        {
        }

        public void Write(IRenderable renderable)
        {
            Renderable = renderable;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class AddRegistrationDependencyAttribute(Type dependencyType)
        : Attribute, IModuleRegistrationEventReceiver
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.AddDependency(dependencyType);
            return Task.CompletedTask;
        }
    }

    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _dependencyExecutions);
            return Task.FromResult<string?>("dependency");
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private sealed class TargetModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _targetExecutions);
            return Task.FromResult<string?>("target");
        }
    }

    private sealed class UnrelatedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _unrelatedExecutions);
            return Task.FromResult<string?>("unrelated");
        }
    }

    private sealed class UnregisteredModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("unregistered");
    }

    [AddRegistrationDependency(typeof(UnregisteredModule))]
    private sealed class InvalidDynamicDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("invalid");
    }

    [AddRegistrationDependency(typeof(DependencyModule))]
    private sealed class RegistrationDependentModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private sealed class ResultDependentSkipModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(async (context, _) =>
            {
                await context.GetModule<DependencyModule>();
                return SkipDecision.Skip("dependency result matched");
            })
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModuleCategory("selected")]
    private sealed class SelectedCategoryModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _categoryExecutions);
            return Task.FromResult<string?>("selected");
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

    [RunIfAll<TrackingCondition>]
    private sealed class ConditionalModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("conditional");
    }

    private sealed class NeverRunCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    [RunIfAll<NeverRunCondition>]
    private sealed class AttributeSkippedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("attribute-skipped");
    }

    private sealed class FluentlySkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithCategory("selected")
            .WithSkipWhen(_ => SkipDecision.Skip("fluent skip"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    private sealed class SkippedDependencyModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithCategory("selected")
            .WithSkipWhen(_ => SkipDecision.Skip("dependency unavailable"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("Dry-run must not execute modules.");
    }

    [ModularPipelines.Attributes.DependsOn<SkippedDependencyModule>]
    private sealed class DependentOnSkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithCategory("selected")
            .Build();

        protected internal override Task<string?> ExecuteAsync(
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
            _ => TimeSpan.Zero,
        });

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration) => Task.CompletedTask;

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(Type moduleType, SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;
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
        using var builder = CreateExecutionBuilder(["--module", nameof(TargetModule)]);

        await builder.ExecutePipelineAsync();

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
        using var builder = CreateExecutionBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            TargetModules = [nameof(TargetModule)],
        });

        await builder.ExecutePipelineAsync();

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
        using var builder = CreateExecutionBuilder(["--skip-module", nameof(UnrelatedModule)]);

        await builder.ExecutePipelineAsync();

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
        using var builder = Pipeline.CreateBuilder(["--categories", "selected"]);
        builder.AddModule<SelectedCategoryModule>();
        builder.AddModule<UnrelatedModule>();

        await builder.ExecutePipelineAsync();

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.RunOnlyCategories).IsEquivalentTo(["selected"]);
            await Assert.That(_categoryExecutions).IsEqualTo(1);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task UnknownArgumentsStillFlowToHostConfiguration()
    {
        using var builder = Pipeline.CreateBuilder(
        [
            "--module", nameof(TargetModule),
            "--custom-setting", "custom-value",
        ]);

        await Assert.That(builder.Options.TargetModules).IsEquivalentTo([nameof(TargetModule)]);
        await Assert.That(builder.Configuration["custom-setting"]).IsEqualTo("custom-value");
    }

    [Test]
    public async Task RepeatedCommaSeparatedAndEqualsValuesAreParsed()
    {
        using var builder = Pipeline.CreateBuilder(
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
        using var builder = CreateExecutionBuilder(
            ["--module", typeof(TargetModule).AssemblyQualifiedName!]);

        await builder.ExecutePipelineAsync();

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
        using var builder = Pipeline.CreateBuilder(["--module", nameof(ConditionalModule)]);
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
        using var builder = Pipeline.CreateBuilder(new PipelineBuilderOptions
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
        using var builder = CreateExecutionBuilder(["--module", "MissingModule"]);

        var exception = await Assert.ThrowsAsync<PipelineValidationException>(
            () => builder.ExecutePipelineAsync());

        await Assert.That(exception!.ValidationResult.Errors.Any(
            error => error.Message.Contains("MissingModule", StringComparison.Ordinal))).IsTrue();
    }

    [Test]
    [Arguments("--list-modules")]
    [Arguments("--validate")]
    public async Task InformationalCommandsDoNotExecuteModules(string command)
    {
        using var builder = CreateExecutionBuilder([command]);

        var summary = await builder.ExecutePipelineAsync();

        using (Assert.Multiple())
        {
            await Assert.That(summary.Results).IsEmpty();
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
            await Assert.That(_targetExecutions).IsEqualTo(0);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PlanAsyncBuildsDependencyOrderedWavesWithoutExecutingModules()
    {
        using var builder = CreateExecutionBuilder();
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
            await Assert.That(plan.EstimatedDuration).IsEqualTo(TimeSpan.FromMinutes(8));
            await Assert.That(_dependencyExecutions).IsEqualTo(0);
            await Assert.That(_targetExecutions).IsEqualTo(0);
            await Assert.That(_unrelatedExecutions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task PlanAsyncIncludesRegistrationTimeDependenciesInWaves()
    {
        using var builder = Pipeline.CreateBuilder();
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
        using var builder = Pipeline.CreateBuilder();
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
    public async Task PlanAsyncEvaluatesAllSkipSourcesAndCascadesDependencies()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunOnlyCategories = ["selected"],
        });
        builder.AddModule<FluentlySkippedModule>();
        builder.AddModule<AttributeSkippedModule>()
            .WithCategory("selected");
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
    public async Task DryRunOptionPrintsPlanAndDoesNotExecuteModules()
    {
        var consoleWriter = new CapturingConsoleWriter();
        using var builder = CreateExecutionBuilder(["--dry-run"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);

        var summary = await builder.ExecutePipelineAsync();
        var output = Render(consoleWriter.Renderable!);

        using (Assert.Multiple())
        {
            await Assert.That(builder.Options.DryRun).IsTrue();
            await Assert.That(summary.Results).IsEmpty();
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
    public async Task ProgrammaticDryRunOptionDoesNotExecuteModules()
    {
        using var builder = CreateExecutionBuilder();
        builder.ConfigurePipelineOptions(options => options with { DryRun = true });

        var summary = await builder.ExecutePipelineAsync();

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
        using var builder = Pipeline.CreateBuilder(["--validate"]);
        builder.AddModule<InvalidDynamicDependencyModule>();

        await Assert.ThrowsAsync<ModuleNotRegisteredException>(
            () => builder.ExecutePipelineAsync());
    }

    [Test]
    public async Task ListModulesUsesFinalizedFluentCategory()
    {
        var consoleWriter = new CapturingConsoleWriter();
        using var builder = Pipeline.CreateBuilder(["--list-modules"]);
        builder.Services.AddSingleton<IConsoleWriter>(consoleWriter);
        builder.AddModule<UnrelatedModule>().WithCategory("configured-category");

        await builder.ExecutePipelineAsync();

        var output = Render(consoleWriter.Renderable!);

        await Assert.That(output).Contains("configured-category");
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
