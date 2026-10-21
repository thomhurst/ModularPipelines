using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.PipelineCli;

public class ModuleIdSelectionTests
{
    [Test]
    public async Task ModuleIdSelectionExecutesTargetAndDependencies()
    {
        var builder = CreateBuilder(["--module", "build"]);

        var summary = await builder.RunAsync();

        await Assert.That(summary.GetResult<BuildModule, string>().Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(summary.GetResult<DependencyModule, string>().Status).IsEqualTo(ModuleStatus.Succeeded);
        await Assert.That(summary.GetResult<UnrelatedModule, string>().Status).IsEqualTo(ModuleStatus.Skipped);
    }

    [Test]
    [Arguments(false, "build")]
    [Arguments(false, "BUILD")]
    [Arguments(true, "build")]
    [Arguments(true, "BUILD")]
    [Arguments(false, nameof(BuildModule))]
    public async Task SelectingModuleIdIncludesDependencyClosure(bool programmatic, string name)
    {
        var builder = CreateBuilder(programmatic ? [] : ["--module", name]);
        if (programmatic)
        {
            builder.ConfigureOptions(options => options with { RunOnlyModules = [name] });
        }

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(BuildModule)].ShouldSkip).IsFalse();
        await Assert.That(modules[typeof(DependencyModule)].ShouldSkip).IsFalse();
        await Assert.That(modules[typeof(UnrelatedModule)].ShouldSkip).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task IgnoringModuleIdOverridesSelection(bool programmatic)
    {
        var builder = CreateBuilder(programmatic ? [] : ["--module", "build", "--skip-module", "BUILD"]);
        if (programmatic)
        {
            builder.ConfigureOptions(options => options with
            {
                RunOnlyModules = ["build"],
                IgnoreModules = ["BUILD"],
            });
        }

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(BuildModule)].ShouldSkip).IsTrue();
        await Assert.That(modules[typeof(DependencyModule)].ShouldSkip).IsFalse();
        await Assert.That(modules[typeof(UnrelatedModule)].ShouldSkip).IsTrue();
    }

    [Test]
    [Arguments("--module")]
    [Arguments("--skip-module")]
    public async Task ModuleIdCollidingWithTypeNameIsAmbiguous(string option)
    {
        var builder = CreateBuilder([option, nameof(UnrelatedModule)]);
        builder.AddModule<CollidingIdModule>();

        var validation = await builder.ValidateAsync();

        await Assert.That(validation.IsValid).IsFalse();
        await Assert.That(validation.Errors.Any(error =>
            error.Message.Contains("ambiguous", StringComparison.OrdinalIgnoreCase))).IsTrue();
    }

    [Test]
    [Arguments("--module")]
    [Arguments("--skip-module")]
    public async Task ModuleIdCollidingWithFullTypeNameSuggestsAssemblyQualifiedNames(string option)
    {
        var builder = CreateBuilder([option, typeof(UnrelatedModule).FullName!]);
        builder.AddModule<FullNameCollidingIdModule>();

        var validation = await builder.ValidateAsync();

        await Assert.That(validation.IsValid).IsFalse();
        await Assert.That(validation.Errors.Any(error =>
            error.Message.Contains("assembly-qualified", StringComparison.OrdinalIgnoreCase)
            && error.Message.Contains(typeof(UnrelatedModule).AssemblyQualifiedName!, StringComparison.Ordinal)
            && error.Message.Contains(typeof(FullNameCollidingIdModule).AssemblyQualifiedName!, StringComparison.Ordinal))).IsTrue();
    }

    [Test]
    [Arguments("--module")]
    [Arguments("--skip-module")]
    public async Task AssemblyQualifiedNameDisambiguatesModuleIdCollision(string option)
    {
        var builder = CreateBuilder([option, typeof(UnrelatedModule).AssemblyQualifiedName!]);
        builder.AddModule<FullNameCollidingIdModule>();

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(UnrelatedModule)].ShouldSkip).IsEqualTo(option == "--skip-module");
        await Assert.That(modules[typeof(FullNameCollidingIdModule)].ShouldSkip).IsEqualTo(option == "--module");
    }

    private static PipelineBuilder CreateBuilder(string[] args)
    {
        var builder = Pipeline.CreateBuilder(args);
        builder.AddModule<DependencyModule>();
        builder.AddModule<BuildModule>();
        builder.AddModule<UnrelatedModule>();
        return builder;
    }

    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("dependency");
    }

    [ModuleId("build")]
    [DependsOn<DependencyModule>]
    private sealed class BuildModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("build");
    }

    private sealed class UnrelatedModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("unrelated");
    }

    [ModuleId("ModularPipelines.UnitTests.PipelineCli.ModuleIdSelectionTests+" + nameof(UnrelatedModule))]
    private sealed class FullNameCollidingIdModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("full name collision");
    }

    [ModuleId(nameof(UnrelatedModule))]
    private sealed class CollidingIdModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("collision");
    }
}
