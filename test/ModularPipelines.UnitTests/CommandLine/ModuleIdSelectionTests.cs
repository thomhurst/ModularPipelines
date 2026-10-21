using System.Reflection;
using System.Reflection.Emit;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.PipelineCli;

public class ModuleIdSelectionTests
{
    [Test]
    [Arguments("--module", false)]
    [Arguments("--module", true)]
    [Arguments("--skip-module", false)]
    [Arguments("--skip-module", true)]
    public async Task DefaultStableModuleIdIsPreserved(string option, bool equalsSyntax)
    {
        var name = ModuleId.FromType(typeof(UnrelatedModule)).Value;
        var builder = CreateBuilder(equalsSyntax ? [$"{option}={name}"] : [option, name]);

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(UnrelatedModule)].ShouldSkip).IsEqualTo(option == "--skip-module");
        await Assert.That(modules[typeof(BuildModule)].ShouldSkip).IsEqualTo(option == "--module");
    }

    [Test]
    [Arguments("--module", false)]
    [Arguments("--module", true)]
    [Arguments("--skip-module", false)]
    [Arguments("--skip-module", true)]
    public async Task CommaContainingModuleIdTakesPrecedenceOverList(string option, bool equalsSyntax)
    {
        const string name = "build," + nameof(UnrelatedModule);
        var builder = CreateBuilder(equalsSyntax ? [$"{option}={name}"] : [option, name]);
        builder.AddModule<CommaIdModule>();

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(CommaIdModule)].ShouldSkip).IsEqualTo(option == "--skip-module");
        await Assert.That(modules[typeof(BuildModule)].ShouldSkip).IsEqualTo(option == "--module");
        await Assert.That(modules[typeof(UnrelatedModule)].ShouldSkip).IsEqualTo(option == "--module");
    }

    [Test]
    [Arguments("--module")]
    [Arguments("--skip-module")]
    public async Task CommaSeparatedModuleListStillResolvesEachName(string option)
    {
        var builder = CreateBuilder([option, $" build, {nameof(UnrelatedModule)}, ", option, nameof(DependencyModule)]);

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToArray();

        await Assert.That(modules.All(module => module.ShouldSkip == (option == "--skip-module"))).IsTrue();
    }

    [Test]
    [Arguments("--module")]
    [Arguments("--skip-module")]
    public async Task CommaOnlyModuleIdIsPreserved(string option)
    {
        var builder = CreateBuilder([option, ",,"]);
        builder.AddModule<CommaOnlyIdModule>();

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var module = plan.Waves.SelectMany(wave => wave.Modules)
            .Single(module => module.Module is CommaOnlyIdModule);

        await Assert.That(module.ShouldSkip).IsEqualTo(option == "--skip-module");
    }

    [Test]
    [Arguments("--module")]
    [Arguments("--skip-module")]
    public async Task UnknownNameInCommaSeparatedListFailsValidation(string option)
    {
        var builder = CreateBuilder([option, "build,MissingModule"]);

        var validation = await builder.ValidateAsync();

        await Assert.That(validation.IsValid).IsFalse();
        await Assert.That(validation.Errors.Any(error =>
            error.Message.Contains("'MissingModule' is not registered", StringComparison.Ordinal))).IsTrue();
    }

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

    [Test]
    [Arguments("--module", false, false)]
    [Arguments("--module", false, true)]
    [Arguments("--skip-module", false, false)]
    [Arguments("--skip-module", false, true)]
    [Arguments("--module", true, false)]
    [Arguments("--skip-module", true, false)]
    public async Task AssemblyQualifiedNameTakesPrecedenceOverModuleId(string option, bool programmatic, bool upperCase)
    {
        var name = typeof(UnrelatedModule).AssemblyQualifiedName!;
        var assembly = AssemblyBuilder.DefineDynamicAssembly(
            new AssemblyName($"ModuleSelectionCollision_{Guid.NewGuid():N}"), AssemblyBuilderAccess.RunAndCollect);
        var type = assembly.DefineDynamicModule("Modules")
            .DefineType("AssemblyNameCollidingIdModule", TypeAttributes.Public, typeof(CollisionModuleBase));
        type.SetCustomAttribute(new CustomAttributeBuilder(
            typeof(ModuleIdAttribute).GetConstructor([typeof(string)])!, [name]));
        var collidingType = type.CreateType()!;
        var selector = upperCase ? name.ToUpperInvariant() : name;
        var builder = CreateBuilder(programmatic ? [] : [option, selector]);
        builder.AddModules(collidingType);
        if (programmatic)
        {
            builder.ConfigureOptions(options => option == "--module"
                ? options with { RunOnlyModules = [selector] }
                : options with { IgnoreModules = [selector] });
        }

        await using var pipeline = await builder.BuildAsync();
        var plan = await pipeline.PlanAsync();
        var modules = plan.Waves.SelectMany(wave => wave.Modules).ToDictionary(module => module.Module.GetType());

        await Assert.That(modules[typeof(UnrelatedModule)].ShouldSkip).IsEqualTo(option == "--skip-module");
        await Assert.That(modules[collidingType].ShouldSkip).IsEqualTo(option == "--module");
    }

    public class CollisionModuleBase : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("assembly name collision");
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

    [ModuleId("build," + nameof(UnrelatedModule))]
    private sealed class CommaIdModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("comma identifier");
    }

    [ModuleId(",,")]
    private sealed class CommaOnlyIdModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("comma-only identifier");
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
