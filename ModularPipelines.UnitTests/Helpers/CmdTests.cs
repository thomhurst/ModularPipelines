using CliWrap.Buffered;
using ModularPipelines.Cmd.Extensions;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests.Helpers;

public class CmdTests : TestBase
{
    private class CmdEchoModule : Module<BufferedCommandResult>
    {
        protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Cmd().Script(new("echo Foo bar!"), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<CmdEchoModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType, Is.EqualTo(ModuleResultType.SuccessfulResult));
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<CmdEchoModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError, Is.Null.Or.Empty);
            Assert.That(moduleResult.Value.StandardOutput.Trim(), Is.EqualTo("Foo bar!"));
        });
    }
}