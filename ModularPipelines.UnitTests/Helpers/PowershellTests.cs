using CliWrap.Buffered;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Powershell.Extensions;

namespace ModularPipelines.UnitTests.Helpers;

public class PowershellTests : TestBase
{
    private class PowershellEchoModule : Module<BufferedCommandResult>
    {
        protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Powershell().Script(new("Write-Host \"Foo bar!\""), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<PowershellEchoModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.IsErrored, Is.False);
            Assert.That(moduleResult.Exception, Is.Null);
            Assert.That(moduleResult.Value, Is.Not.Null);
        });
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<PowershellEchoModule>();

        var moduleResult = await module;
        
        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError, Is.Null.Or.Empty);
            Assert.That(moduleResult.Value.StandardOutput.Trim(), Is.EqualTo("Foo bar!"));
        });
    }
}