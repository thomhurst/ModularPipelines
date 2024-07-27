using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Base64Tests : TestBase
{
    private class ToBase64Module : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.ToBase64String("Foo bar!");
        }
    }

    [Test]
    public async Task To_Base64_Has_Not_Errored()
    {
        var module = await RunModule<ToBase64Module>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task To_Base64_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToBase64Module>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).Is.EqualTo("Rm9vIGJhciE=");
    }

    private class FromBase64Module : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.FromBase64String("Rm9vIGJhciE=");
        }
    }

    [Test]
    public async Task From_Base64_Has_Not_Errored()
    {
        var module = await RunModule<FromBase64Module>();

        var moduleResult = await module;
        
        await using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).Is.Null();
            await Assert.That(moduleResult.Value).Is.Not.Null();
        }
    }

    [Test]
    public async Task From_Base64_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<FromBase64Module>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).Is.EqualTo("Foo bar!");
    }
}