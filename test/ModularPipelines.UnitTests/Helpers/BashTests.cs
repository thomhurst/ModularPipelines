using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.Helpers;

public class BashTests : TestBase
{
    private class BashCommandModule : IModule<CommandResult>
    {
        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Bash.Command(new("echo \"Foo bar!\""), cancellationToken: cancellationToken);
        }
    }

    private class BashScriptModule : IModule<CommandResult>
    {
        public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var file = context.Git().RootDirectory.FindFile(x => x.Name == "BashTest.sh");
            return await context.Bash.FromFile(new BashFileOptions(file!), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await RunModuleWithResult<BashCommandModule, CommandResult>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
            await Assert.That(moduleResult.Value).IsNotNull();
        }
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await RunModuleWithResult<BashCommandModule, CommandResult>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).IsEqualTo("Foo bar!");
        }
    }

    [Test]
    [LinuxOnlyTest]
    public async Task Standard_Output_From_Script_Equals_Foo_Bar()
    {
        var moduleResult = await RunModuleWithResult<BashScriptModule, CommandResult>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.Value!.StandardError).IsNull().Or.IsEmpty();
            await Assert.That(moduleResult.Value.StandardOutput.Trim()).IsEqualTo("Foo bar!");
        }
    }
}
