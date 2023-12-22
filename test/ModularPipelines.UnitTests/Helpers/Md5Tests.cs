using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

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

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.Success));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task To_Md5_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToMd5Module>();

        var moduleResult = await module;

        Assert.That(moduleResult.Value, Is.EqualTo("b9c291e3274aa5c8010a7c5c22a4e6dd"));
    }
}