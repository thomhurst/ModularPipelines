using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.Helpers;

public class BashTests : TestBase
{
    private class BashCommandModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Shell.Bash.Command(new("echo \"Foo bar!\""), cancellationToken: cancellationToken);
        }
    }

    private class BashScriptModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var file = context.Git().RootDirectory.FindFile(x => x.Name == "BashTest.sh");
            return await context.Shell.Bash.FromFile(new BashFileOptions(file!), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<BashCommandModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<BashCommandModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    [LinuxOnlyTest]
    public async Task Standard_Output_From_Script_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<BashScriptModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }
}
