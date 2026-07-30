using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.PipelineCli;

[TUnit.Core.NotInParallel(nameof(PipelineCommandLineTests))]
public class PipelineCommandLineTests
{
    private static int _dependencyExecutions;
    private static int _targetExecutions;
    private static int _unrelatedExecutions;
    private static int _categoryExecutions;

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

    [Before(Test)]
    public void ResetCounters()
    {
        _dependencyExecutions = 0;
        _targetExecutions = 0;
        _unrelatedExecutions = 0;
        _categoryExecutions = 0;
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
    public async Task PipelineCommandsAreMutuallyExclusive()
    {
        await Assert.That(() => Pipeline.CreateBuilder(["--list-modules", "--validate"]))
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
