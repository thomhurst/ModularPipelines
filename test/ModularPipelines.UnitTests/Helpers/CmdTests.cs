using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using ModularPipelines.UnitTests.Attributes;

namespace ModularPipelines.UnitTests.Helpers;

[WindowsOnlyTest]
public class CmdTests : TestBase
{
    private class CmdEchoModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Tools.Cmd.RunAsync(
                "echo Foo bar!",
                new CommandExecutionOptions { ThrowOnNonZeroExitCode = true },
                cancellationToken);
        }
    }

    private class CmdFileModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var file = GetCmdTestFile(context);
            var options = new CmdFileOptions("missing.cmd") with { FilePath = file.Path };
            return await context.Tools.Cmd.RunFileAsync(options, cancellationToken: cancellationToken);
        }
    }

    private class CmdOptionsModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return await context.Tools.Cmd.RunAsync(
                new CmdScriptOptions("echo Foo bar!"),
                new CommandExecutionOptions { ThrowOnNonZeroExitCode = true },
                cancellationToken);
        }
    }

    private class CmdStringFileModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var file = GetCmdTestFile(context);
            return await context.Tools.Cmd.RunFileAsync(file.Path, cancellationToken: cancellationToken);
        }
    }

    private class CmdToolOverrideFileModule : Module<CommandResult>
    {
        protected internal override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var file = GetCmdTestFile(context);
            var options = new CmdFileOptions("missing.cmd") { Tool = file.Path };
            return await context.Tools.Cmd.RunFileAsync(options, cancellationToken: cancellationToken);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<CmdEchoModule>();

        await ModuleResultAssertions.AssertSuccessWithValue(moduleResult);
    }

    [Test]
    public async Task Standard_Output_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<CmdEchoModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task Standard_Output_From_File_Equals_Foo_Bar()
    {
        var moduleResult = await await RunModule<CmdFileModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task Options_Record_Overload_Produces_Expected_Output()
    {
        var moduleResult = await await RunModule<CmdOptionsModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task File_String_Overload_Produces_Expected_Output()
    {
        var moduleResult = await await RunModule<CmdStringFileModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    [Test]
    public async Task File_Options_Preserve_Explicit_Tool()
    {
        var moduleResult = await await RunModule<CmdToolOverrideFileModule>();

        await ModuleResultAssertions.AssertCommandOutput(moduleResult, TestConstants.TestString);
    }

    private static ModularPipelines.FileSystem.File GetCmdTestFile(IModuleContext context)
    {
        return context.Files.GetFile(Path.Combine(
            TestContext.OutputDirectory!,
            "Data",
            "CmdTest %PATH% & echo injected.cmd"));
    }
}
