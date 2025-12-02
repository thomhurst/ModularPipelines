using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Sha256Tests : TestBase
{
    private class ToSha256Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha256("Foo bar!");
        }
    }

    [Test]
    public async Task To_Sha256_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ToSha256Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task To_Sha256_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<ToSha256Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("d80c14a132a9ae008c78db4ee4cbc46b015b5e0f018f6b0a3e4ea5041176b852");
    }
}
