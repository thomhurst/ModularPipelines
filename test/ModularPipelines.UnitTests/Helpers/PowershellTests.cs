using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class PowershellTests : TestBase
{
    private class PowershellEchoModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Shell.PowerShell.Script(new("Write-Host \"Foo bar!\""), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<PowershellEchoModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<PowershellEchoModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }
}
