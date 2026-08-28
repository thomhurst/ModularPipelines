using ModularPipelines.Plugins;

namespace ModularPipelines.Testing.UnitTests;

[TUnit.Core.NotInParallel(nameof(PluginTestHelperTests))]
public class PluginTestHelperTests
{
    [Test]
    public async Task IsolatedRegistryStartsEmpty()
    {
        using var scope = PluginTestHelper.IsolatedRegistry();

        await Assert.That(PluginRegistry.Plugins).IsEmpty();
    }
}
