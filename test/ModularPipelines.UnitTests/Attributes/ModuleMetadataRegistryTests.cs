using ModularPipelines.Configuration;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleMetadataRegistryTests
{
    private class ModuleA : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
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

    private static ModuleMetadataRegistry CreateRegistry()
        => new(Microsoft.Extensions.Options.Options.Create(new ModuleRegistrationOptions()));

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
}
