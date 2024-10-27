using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Sha1Tests : TestBase
{
    private class ToSha1Module : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha1("Foo bar!");
        }
    }

    [Test]
    public async Task To_Sha1_Has_Not_Errored()
    {
        var module = await RunModule<ToSha1Module>();

        var moduleResult = await module;
        
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
        var module = await RunModule<ToSha1Module>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).IsEqualTo("cc3626c5ad2e3aff0779dc63e80555c463fd99dc");
    }
}