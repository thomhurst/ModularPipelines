using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Sha256Tests : TestBase
{
    private class ToSha256Module : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha256("Foo bar!");
        }
    }

    [Test]
    public async Task To_Sha256_Has_Not_Errored()
    {
        var module = await RunModule<ToSha256Module>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task To_Sha256_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToSha256Module>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).Is.EqualTo("d80c14a132a9ae008c78db4ee4cbc46b015b5e0f018f6b0a3e4ea5041176b852");
    }
}