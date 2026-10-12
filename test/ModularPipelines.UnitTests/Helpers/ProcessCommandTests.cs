using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Helpers;

public class ProcessCommandTests
{
    [Test]
    public async Task ResolveFileName_Finds_Extensionless_Tool_On_Path()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var resolved = ProcessCommand.ResolveFileName("cmd");

        await Assert.That(Path.IsPathFullyQualified(resolved)).IsTrue();
        await Assert.That(Path.GetFileName(resolved)).IsEqualTo("cmd.exe").IgnoringCase();
    }

    [Test]
    [Arguments("tool.exe")]
    [Arguments("modular-pipelines-missing-tool")]
    public async Task ResolveFileName_Leaves_Names_With_Extension_Or_Not_Found_Unchanged(string tool)
    {
        await Assert.That(ProcessCommand.ResolveFileName(tool)).IsEqualTo(tool);
    }
}
