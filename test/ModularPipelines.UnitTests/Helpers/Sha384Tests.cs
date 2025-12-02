using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class Sha384Tests : TestBase
{
    private class ToSha384Module : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return context.Hasher.Sha384("Foo bar!");
        }
    }

    [Test]
    public async Task To_Sha384_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ToSha384Module>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task To_Sha384_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<ToSha384Module>();
        await Assert.That(moduleResult.Value).IsEqualTo("bb338a277da65d5663467d5fd98aa67349506150cd1287597b0eaa0f0988d2b22c33504fd85dd0b8c99ce8cc50666f88");
    }
}
