using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Md5Tests : TestBase
{
    private class ToMd5Module : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Md5("Foo bar!");
        }
    }

    [Test]
    public async Task To_Md5_Has_Not_Errored()
    {
        var module = await RunModule<ToMd5Module>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task To_Md5_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToMd5Module>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).Is.EqualTo("b9c291e3274aa5c8010a7c5c22a4e6dd");
    }
}