using System.Collections.Frozen;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Configuration;

public class ModuleConfigureTests
{
    private sealed class CountingModule : Module<string>
    {
        public int ConfigureCount { get; private set; }

        protected override void Configure(ModuleConfigurationBuilder module)
        {
            ConfigureCount++;
            module.WithTags("cached-tag");
        }

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    private class TestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    private class ConfiguredModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithAlwaysRun();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [ModularPipelines.Attributes.NotInParallel("attribute-lock")]
    [Priority(ModulePriority.High)]
    [ExecutionHint(ExecutionHint.CpuBound)]
    [ModuleTag("attribute-tag")]
    [ModuleCategory("attribute-category")]
    [ModularPipelines.Attributes.DependsOn<TestModule>]
    private class UnifiedConfigurationModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithNotInParallel("fluent-lock")
            .WithPriority(ModulePriority.Critical)
            .WithExecutionHint(ExecutionHint.IoBound)
            .WithTags("fluent-tag")
            .WithCategory("fluent-category");

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [ModularPipelines.Attributes.DependsOn<TestModule>]
    private class RequiredAndOptionalDependencyModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) =>
            module.DependsOnOptional<TestModule>();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [Test]
    public async Task Module_DefaultConfiguration_HasDefaultValues()
    {
        var module = new TestModule();
        var config = ((IModule) module).Configuration;

        using (Assert.Multiple())
        {
            await Assert.That(config.Timeout).IsNull();
            await Assert.That(config.AlwaysRun).IsFalse();
            await Assert.That(config.Dependencies).IsEmpty();
        }
    }

    [Test]
    public async Task Module_OverriddenConfigure_ReturnsCustomConfig()
    {
        var module = new ConfiguredModule();
        var config = ((IModule) module).Configuration;

        await Assert.That(config.Timeout).IsEqualTo(TimeSpan.FromSeconds(60));
        await Assert.That(config.AlwaysRun).IsTrue();
    }

    [Test]
    public async Task Module_Configuration_IsCached()
    {
        var module = new ConfiguredModule();
        var config1 = ((IModule) module).Configuration;
        var config2 = ((IModule) module).Configuration;

        await Assert.That(config1).IsSameReferenceAs(config2);
    }

    [Test]
    public async Task ModuleActivator_Initializes_Configuration_Once()
    {
        using var services = new ServiceCollection().BuildServiceProvider();
        var module = (CountingModule) new ModuleActivator()
            .CreateModule(typeof(CountingModule), services);

        var configurations = await Task.WhenAll(
            Enumerable.Range(0, 10)
                .Select(_ => Task.Run(() => ((IModule) module).Configuration)));
        var tags = configurations[0].Tags;

        using (Assert.Multiple())
        {
            await Assert.That(module.ConfigureCount).IsEqualTo(1);
            await Assert.That(configurations.All(
                    configuration => ReferenceEquals(configuration, configurations[0])))
                .IsTrue();
            await Assert.That(tags is FrozenSet<string>).IsTrue();
            await Assert.That(tags).Contains("cached-tag");
            await Assert.That(((IModule) module).Configuration.Tags).IsSameReferenceAs(tags);
        }
    }

    [Test]
    public async Task Module_Configuration_Combines_Attributes_Into_Fluent_Model()
    {
        var config = ((IModule) new UnifiedConfigurationModule()).Configuration;

        using (Assert.Multiple())
        {
            await Assert.That(config.ParallelConstraintKeys).IsEquivalentTo(new[] { "fluent-lock" });
            await Assert.That(config.Priority).IsEqualTo(ModulePriority.Critical);
            await Assert.That(config.ExecutionHint).IsEqualTo(ExecutionHint.IoBound);
            await Assert.That(config.Tags).IsEquivalentTo(new[] { "attribute-tag", "fluent-tag" });
            await Assert.That(config.Category).IsEqualTo("fluent-category");
            await Assert.That(config.Dependencies.Select(dependency => dependency.ModuleType))
                .Contains(typeof(TestModule));
        }
    }

    [Test]
    public async Task Module_Configuration_Uses_Strictest_Duplicate_Dependency()
    {
        var dependency = ((IModule) new RequiredAndOptionalDependencyModule()).Configuration.Dependencies.Single();

        await Assert.That(dependency.Kind).IsEqualTo(DependencyType.Required);
        await Assert.That(dependency.IsOptional).IsFalse();
    }

    [Test]
    public async Task ConfigurationConstructionApisAreNotPublic()
    {
        const System.Reflection.BindingFlags publicStatic =
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static;
        const System.Reflection.BindingFlags publicInstance =
            System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

        using (Assert.Multiple())
        {
            await Assert.That(typeof(ModuleConfiguration).GetConstructors()).IsEmpty();
            await Assert.That(typeof(ModuleConfiguration).GetProperty("Default", publicStatic)).IsNull();
            await Assert.That(typeof(ModuleConfiguration).GetMethod("Create", publicStatic)).IsNull();
            await Assert.That(typeof(ModuleConfigurationBuilder).GetMethod("Build", publicInstance)).IsNull();
        }
    }
}
