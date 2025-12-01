using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Sha1Tests : TestBase
{
    private class ToSha1Module : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha1("Foo bar!");
        }
    }

    [Test]
    public async Task To_Sha1_Has_Not_Errored()
    {
        var moduleResult = await RunModuleWithResult<ToSha1Module, string>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task To_Sha1_Output_Equals_Foo_Bar()
    {
        var moduleResult = await RunModuleWithResult<ToSha1Module, string>();
        await Assert.That(moduleResult.Value).IsEqualTo("cc3626c5ad2e3aff0779dc63e80555c463fd99dc");
    }
}
