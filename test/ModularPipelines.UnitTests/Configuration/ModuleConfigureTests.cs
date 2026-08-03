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

        protected override ModuleConfiguration Configure()
        {
            ConfigureCount++;
            return ModuleConfiguration.Create()
                .WithTags("cached-tag")
                .Build();
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
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(60))
            .WithAlwaysRun()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [ModularPipelines.Attributes.NotInParallel("attribute-lock")]
    [Priority(ModulePriority.High)]
    [ExecutionHint(ExecutionType.CpuIntensive)]
    [ModuleTag("attribute-tag")]
    [ModuleCategory("attribute-category")]
    [ModularPipelines.Attributes.DependsOn<TestModule>]
    private class UnifiedConfigurationModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithNotInParallel("fluent-lock")
            .WithPriority(ModulePriority.Critical)
            .WithExecutionHint(ExecutionType.IoIntensive)
            .WithTags("fluent-tag")
            .WithCategory("fluent-category")
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [ModularPipelines.Attributes.DependsOn<TestModule>]
    private class RequiredAndOptionalDependencyModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .DependsOnOptional<TestModule>()
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("test");
    }

    [Test]
    public async Task Module_DefaultConfiguration_ReturnsDefault()
    {
        var module = new TestModule();
        var config = ((IModule) module).Configuration;

        await Assert.That(config).IsSameReferenceAs(ModuleConfiguration.Default);
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
            await Assert.That(config.ExecutionType).IsEqualTo(ExecutionType.IoIntensive);
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
}
