using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class HexTests : TestBase
{
    private class ToHexModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.ToHex("Foo bar!");
        }
    }

    [Test]
    public async Task To_Hex_Has_Not_Errored()
    {
        var module = await RunModule<ToHexModule>();

        var moduleResult = await module;
        
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task To_Hex_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<ToHexModule>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).IsEqualTo("466f6f2062617221");
    }

    private class FromHexModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.FromHex("466f6f2062617221");
        }
    }

    [Test]
    public async Task From_Hex_Has_Not_Errored()
    {
        var module = await RunModule<FromHexModule>();

        var moduleResult = await module;
        
        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task From_Hex_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<FromHexModule>();

        var moduleResult = await module;
        await Assert.That(moduleResult.Value).IsEqualTo("Foo bar!");
    }
}