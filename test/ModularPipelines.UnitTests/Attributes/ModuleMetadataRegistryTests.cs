using System.Reflection;
using ModularPipelines.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleMetadataRegistryTests
{
    private class ModuleA : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string>("A");
    }

    private sealed class DirectTaggedModule : IModule, ITaggedModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration { get; } = ModuleConfiguration.Create().Build();

        public Task<IModuleResult> ResultTask => null!;

        public IReadOnlySet<string> Tags { get; } = new HashSet<string> { "direct-tag" };

        public string? Category => "direct-category";

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    [ModuleTag("attribute-tag")]
    [ModuleCategory("attribute-category")]
    private sealed class DirectAttributedModule : IModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration { get; } = ModuleConfiguration.Default;

        public Task<IModuleResult> ResultTask => null!;

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    private sealed class CachedAttribute : Attribute
    {
    }

    [Cached]
    private sealed class DirectCachedModule : IModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration { get; } = ModuleConfiguration.Default;

        public Task<IModuleResult> ResultTask => null!;

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    [Cached]
    [Cached]
    private sealed class DirectMultiplyAttributedModule : IModule
    {
        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration { get; } = ModuleConfiguration.Default;

        public Task<IModuleResult> ResultTask => null!;

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    private sealed class ConfigurationCountingModule : IModule
    {
        public int ConfigurationReadCount { get; private set; }

        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration
        {
            get
            {
                ConfigurationReadCount++;
                return ModuleConfiguration.Default;
            }
        }

        public Task<IModuleResult> ResultTask => null!;

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    private static ModuleMetadataRegistry CreateRegistry()
        => new(
            Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions()),
            new ModuleAttributeEventService());

    private static ModuleMetadataRegistry CreateRegistry(IModuleAttributeEventService attributeEventService)
        => new(
            Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions()),
            attributeEventService);

    [Test]
    public async Task SetMetadata_GetMetadata_ReturnsValue()
    {
        var registry = CreateRegistry();

        registry.SetMetadata(typeof(ModuleA), "key", "value");

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsEqualTo("value");
    }

    [Test]
    public async Task GetMetadata_NotSet_ReturnsNull()
    {
        var registry = CreateRegistry();

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task SetMetadata_OverwritesExisting()
    {
        var registry = CreateRegistry();

        registry.SetMetadata(typeof(ModuleA), "key", "value1");
        registry.SetMetadata(typeof(ModuleA), "key", "value2");

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsEqualTo("value2");
    }

    [Test]
    public async Task FinalizeMetadata_PreservesDirectTaggedModuleMetadata()
    {
        var registry = CreateRegistry();
        var module = new DirectTaggedModule();

        registry.FinalizeMetadata(typeof(DirectTaggedModule), module);

        await Assert.That(registry.GetTags(typeof(DirectTaggedModule))).Contains("direct-tag");
        await Assert.That(registry.GetCategory(typeof(DirectTaggedModule))).IsEqualTo("direct-category");
    }

    [Test]
    public async Task FinalizeMetadata_PreservesDirectModuleAttributes()
    {
        var registry = CreateRegistry();
        var module = new DirectAttributedModule();

        registry.FinalizeMetadata(typeof(DirectAttributedModule), module);

        await Assert.That(registry.GetTags(typeof(DirectAttributedModule))).Contains("attribute-tag");
        await Assert.That(registry.GetCategory(typeof(DirectAttributedModule))).IsEqualTo("attribute-category");
    }

    [Test]
    public async Task AttributeQueries_ShareCachedEventAttributeInstances()
    {
        var attributeEventService = new ModuleAttributeEventService();
        var registry = CreateRegistry(attributeEventService);

        var contextAttribute = attributeEventService
            .GetAttributes(typeof(DirectCachedModule))
            .OfType<CachedAttribute>()
            .Single();
        var first = registry.GetAttribute<CachedAttribute>(typeof(DirectCachedModule));
        var second = registry.GetAttributes<CachedAttribute>(typeof(DirectCachedModule)).Single();

        await Assert.That(ReferenceEquals(contextAttribute, first)).IsTrue();
        await Assert.That(ReferenceEquals(first, second)).IsTrue();
        await Assert.That(registry.HasAttribute<CachedAttribute>(typeof(DirectCachedModule))).IsTrue();
    }

    [Test]
    public async Task GetAttribute_WithMultipleMatches_PreservesAmbiguousMatchError()
    {
        var registry = CreateRegistry();

        await Assert.That(() => registry.GetAttribute<CachedAttribute>(typeof(DirectMultiplyAttributedModule)))
            .Throws<AmbiguousMatchException>();
    }

    [Test]
    public async Task FinalizeMetadata_WhenAlreadyFinalized_DoesNotReadConfigurationAgain()
    {
        var registry = CreateRegistry();
        var module = new ConfigurationCountingModule();

        registry.FinalizeMetadata(typeof(ConfigurationCountingModule), module);
        registry.FinalizeMetadata(typeof(ConfigurationCountingModule), module);

        await Assert.That(module.ConfigurationReadCount).IsEqualTo(1);
    }
}
