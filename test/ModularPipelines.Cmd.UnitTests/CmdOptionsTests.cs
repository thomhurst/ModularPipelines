using System.Text;
using ModularPipelines.Cmd.Extensions;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Cmd.UnitTests;

public class CmdOptionsTests : TestBase
{
    [Test]
    [Arguments(false, false, false, "cmd /q /c echo hi")]
    [Arguments(true, false, false, "cmd /q /u /c echo hi")]
    [Arguments(false, true, false, "cmd /q /a /c echo hi")]
    [Arguments(false, false, true, "cmd /q /d /c echo hi")]
    [Arguments(true, false, true, "cmd /q /u /d /c echo hi")]
    [Arguments(false, true, true, "cmd /q /a /d /c echo hi")]
    [Arguments(true, true, true, "cmd /q /u /a /d /c echo hi")]
    public async Task Switches_Render_Before_StopAfter_And_Script(
        bool unicode, bool ansi, bool disableAutoRun, string expected)
    {
        var rendered = await RenderCommand(new CmdScriptOptions("echo hi")
        {
            DisableEcho = true,
            Unicode = unicode,
            Ansi = ansi,
            DisableAutoRunCommands = disableAutoRun,
        });

        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    public async Task StopAfter_False_Omits_Only_The_Exit_Switch()
    {
        var rendered = await RenderCommand(new CmdScriptOptions("echo hi")
        {
            DisableEcho = true,
            Unicode = true,
            Ansi = true,
            DisableAutoRunCommands = true,
            StopAfter = false,
        });

        await Assert.That(rendered).IsEqualTo("cmd /q /u /a /d echo hi");
    }

    [Test]
    [Arguments(true, false, false)]
    [Arguments(false, true, false)]
    [Arguments(false, false, true)]
    [Arguments(true, false, true)]
    [Arguments(false, true, true)]
    public async Task Switches_Execute_Script_On_Windows(bool unicode, bool ansi, bool disableAutoRun)
    {
        if (!OperatingSystem.IsWindows())
        {
            Skip.Test("Command Prompt execution requires Windows.");
            return;
        }

        var (cmd, _) = await GetService<ICmdContext>(services => services.RegisterCmdContext());
        var result = await cmd.RunAsync(new CmdScriptOptions("echo hi")
        {
            DisableEcho = true,
            Unicode = unicode,
            Ansi = ansi,
            DisableAutoRunCommands = disableAutoRun,
        }, new CommandExecutionOptions
        {
            OutputEncoding = unicode ? Encoding.Unicode : Encoding.UTF8,
            ExecutionTimeout = TimeSpan.FromSeconds(15),
        });

        await Assert.That(result.ExitCode).IsEqualTo(0);
        await Assert.That(result.StandardOutput.Trim()).IsEqualTo("hi");
    }
}
