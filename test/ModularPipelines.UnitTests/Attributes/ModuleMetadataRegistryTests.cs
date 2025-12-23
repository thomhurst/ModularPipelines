using ModularPipelines.Context;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Attributes;

public class ModuleMetadataRegistryTests
{
    private class ModuleA : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => Task.FromResult<string?>("A");
    }

    [Test]
    public async Task SetMetadata_GetMetadata_ReturnsValue()
    {
        var registry = new ModuleMetadataRegistry();

        registry.SetMetadata(typeof(ModuleA), "key", "value");

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsEqualTo("value");
    }

    [Test]
    public async Task GetMetadata_NotSet_ReturnsNull()
    {
        var registry = new ModuleMetadataRegistry();

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsNull();
    }

    [Test]
    public async Task SetMetadata_OverwritesExisting()
    {
        var registry = new ModuleMetadataRegistry();

        registry.SetMetadata(typeof(ModuleA), "key", "value1");
        registry.SetMetadata(typeof(ModuleA), "key", "value2");

        var result = registry.GetMetadata<string>(typeof(ModuleA), "key");
        await Assert.That(result).IsEqualTo("value2");
    }
}
