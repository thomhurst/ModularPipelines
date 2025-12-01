using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Base64Tests : TestBase
{
    private class ToBase64Module : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.ToBase64String("Foo bar!");
        }
    }

    [Test]
    public async Task To_Base64_Has_Not_Errored()
    {
        var moduleResult = await RunModuleWithResult<ToBase64Module, string>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task To_Base64_Output_Equals_Foo_Bar()
    {
        var moduleResult = await RunModuleWithResult<ToBase64Module, string>();
        await Assert.That(moduleResult.Value).IsEqualTo("Rm9vIGJhciE=");
    }

    private class FromBase64Module : IModule<string>
    {
        public async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Base64.FromBase64String("Rm9vIGJhciE=");
        }
    }

    [Test]
    public async Task From_Base64_Has_Not_Errored()
    {
        var moduleResult = await RunModuleWithResult<FromBase64Module, string>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task From_Base64_Output_Equals_Foo_Bar()
    {
        var moduleResult = await RunModuleWithResult<FromBase64Module, string>();
        await Assert.That(moduleResult.Value).IsEqualTo("Foo bar!");
    }
}
