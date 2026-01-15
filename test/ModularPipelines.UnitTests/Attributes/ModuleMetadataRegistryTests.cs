using Microsoft.Extensions.Options;
using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
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
}
