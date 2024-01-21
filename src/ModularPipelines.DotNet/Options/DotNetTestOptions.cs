using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetTestOptions : DotNetOptions
{
    public DotNetTestOptions(
        string projectSolutionDirectoryDllExe
    )
    {
        CommandParts = ["test", "[<PROJECT> | <SOLUTION> | <DIRECTORY> | <DLL> | <EXE>]"];

        ProjectSolutionDirectoryDllExe = projectSolutionDirectoryDllExe;
    }

    public DotNetTestOptions()
    {
        CommandParts = ["test", "[<PROJECT> | <SOLUTION> | <DIRECTORY> | <DLL> | <EXE>]"];
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT> | <SOLUTION> | <DIRECTORY> | <DLL> | <EXE>]")]
    public string? ProjectSolutionDirectoryDllExe { get; set; }

    [CommandSwitch("--test-adapter-path")]
    public string? TestAdapterPath { get; set; }

    [CommandSwitch("--arch")]
    public string? Architecture { get; set; }

    [BooleanCommandSwitch("--blame")]
    public bool? Blame { get; set; }

    [BooleanCommandSwitch("--blame-crash")]
    public bool? BlameCrash { get; set; }

    [CommandSwitch("--blame-crash-dump-type")]
    public string? BlameCrashDumpType { get; set; }

    [BooleanCommandSwitch("--blame-crash-collect-always")]
    public bool? BlameCrashCollectAlways { get; set; }

    [BooleanCommandSwitch("--blame-hang")]
    public bool? BlameHang { get; set; }

    [CommandSwitch("--blame-hang-dump-type")]
    public string? BlameHangDumpType { get; set; }

    [CommandSwitch("--blame-hang-timeout")]
    public string? BlameHangTimeout { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--collect")]
    public string? Collect { get; set; }

    [CommandSwitch("--diag")]
    public string? Diag { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [CommandSwitch("--environment")]
    public IEnumerable<string>? Environment { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--logger")]
    public IEnumerable<string>? Logger { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public bool? Nologo { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--results-directory")]
    public string? ResultsDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [CommandSwitch("--settings")]
    public string? SettingsFile { get; set; }

    [BooleanCommandSwitch("--list-tests")]
    public bool? ListTests { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
