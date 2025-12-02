using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class HexTests : TestBase
{
    private class ToHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.ToHex("Foo bar!");
        }
    }

    [Test]
    public async Task To_Hex_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ToHexModule>();

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
        var moduleResult = await await RunModule<ToHexModule>();
        await Assert.That(moduleResult.Value).IsEqualTo("466f6f2062617221");
    }

    private class FromHexModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hex.FromHex("466f6f2062617221");
        }
    }

    [Test]
    public async Task From_Hex_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<FromHexModule>();

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
        var moduleResult = await await RunModule<FromHexModule>();
        await Assert.That(moduleResult.Value).IsEqualTo("Foo bar!");
    }
}
