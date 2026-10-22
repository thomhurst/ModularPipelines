using ModularPipelines.Context;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Helpers;

public class PowerShellCommandLineTests : TestBase
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Defaults_Precede_Script_Or_File(bool useFile)
    {
        var builder = await GetService<ICommandLineBuilder>();
        PowerShellOptions options = useFile
            ? new PowerShellFileOptions("path with spaces/script.ps1")
            : new PowerShellScriptOptions("Write-Output 'hello world'");

        var result = builder.Build(options);
        string[] expected =
        [
            "-NoProfile",
            "-NonInteractive",
            useFile ? "-File" : "-Command",
            useFile ? "path with spaces/script.ps1" : "Write-Output 'hello world'",
        ];

        await Assert.That(result.Tool).IsEqualTo("pwsh");
        await Assert.That(result.Arguments).IsEquivalentTo(expected,
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }

    [Test]
    [Arguments(false, false, false)]
    [Arguments(false, false, true)]
    [Arguments(false, true, false)]
    [Arguments(false, true, true)]
    [Arguments(true, false, false)]
    [Arguments(true, false, true)]
    [Arguments(true, true, false)]
    [Arguments(true, true, true)]
    public async Task Startup_Flags_Can_Be_Disabled_Independently(bool useFile, bool noProfile, bool nonInteractive)
    {
        var builder = await GetService<ICommandLineBuilder>();
        PowerShellOptions defaults = useFile
            ? new PowerShellFileOptions("script.ps1")
            : new PowerShellScriptOptions("Write-Output 'hello'");
        var options = defaults with { NoProfile = noProfile, NonInteractive = nonInteractive };

        var result = builder.Build(options);
        var expected = new List<string>();
        if (noProfile)
        {
            expected.Add("-NoProfile");
        }

        if (nonInteractive)
        {
            expected.Add("-NonInteractive");
        }

        expected.Add(useFile ? "-File" : "-Command");
        expected.Add(useFile ? "script.ps1" : "Write-Output 'hello'");

        await Assert.That(result.Arguments).IsEquivalentTo(expected,
            TUnit.Assertions.Enums.CollectionOrdering.Matching);
    }
}
