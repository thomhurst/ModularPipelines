using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Attributes;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Helpers;

public class BashTests : TestBase
{
    private class BashCommandModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return await context.Bash.Command(new("echo \"Foo bar!\""), cancellationToken: cancellationToken);
        }
    }

    private class BashScriptModule : Module<CommandResult>
    {
        protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            var file = context.Git().RootDirectory.FindFile(x => x.Name == "BashTest.sh");
            return await context.Bash.FromFile(new BashFileOptions(file!), cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<BashCommandModule>();

        var moduleResult = await module;
        
        await Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(moduleResult.Exception).Is.Null();
            Assert.That(moduleResult.Value).Is.Not.Null();
        });
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var module = await RunModule<BashCommandModule>();

        var moduleResult = await module;
        
        await Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError).Is.Null().Or.Is.Empty();
            Assert.That(moduleResult.Value.StandardOutput.Trim()).Is.EqualTo("Foo bar!");
        });
    }

    [Test]
    [LinuxOnlyTest]
    public async Task Standard_Output_From_Script_Equals_Foo_Bar()
    {
        var module = await RunModule<BashScriptModule>();

        var moduleResult = await module;
        
        await Assert.Multiple(() =>
        {
            Assert.That(moduleResult.Value!.StandardError).Is.Null().Or.Is.Empty();
            Assert.That(moduleResult.Value.StandardOutput.Trim()).Is.EqualTo("Foo bar!");
        });
    }
}