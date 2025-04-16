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
    public virtual string? TestAdapterPath { get; set; }

    [CommandSwitch("--arch")]
    public virtual string? Architecture { get; set; }

    [BooleanCommandSwitch("--blame")]
    public virtual bool? Blame { get; set; }

    [BooleanCommandSwitch("--blame-crash")]
    public virtual bool? BlameCrash { get; set; }

    [CommandSwitch("--blame-crash-dump-type")]
    public virtual string? BlameCrashDumpType { get; set; }

    [BooleanCommandSwitch("--blame-crash-collect-always")]
    public virtual bool? BlameCrashCollectAlways { get; set; }

    [BooleanCommandSwitch("--blame-hang")]
    public virtual bool? BlameHang { get; set; }

    [CommandSwitch("--blame-hang-dump-type")]
    public virtual string? BlameHangDumpType { get; set; }

    [CommandSwitch("--blame-hang-timeout")]
    public virtual string? BlameHangTimeout { get; set; }

    [CommandSwitch("--configuration")]
    public virtual string? Configuration { get; set; }

    [CommandSwitch("--collect")]
    public virtual string? Collect { get; set; }

    [CommandSwitch("--diag")]
    public virtual string? Diag { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [CommandSwitch("--environment")]
    public virtual IEnumerable<string>? Environment { get; set; }

    [CommandSwitch("--filter")]
    public virtual string? Filter { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--logger")]
    public virtual IEnumerable<string>? Logger { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public virtual bool? Nologo { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CommandSwitch("--os")]
    public virtual string? Os { get; set; }

    [CommandSwitch("--results-directory")]
    public virtual string? ResultsDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CommandSwitch("--settings")]
    public virtual string? SettingsFile { get; set; }

    [BooleanCommandSwitch("--list-tests")]
    public virtual bool? ListTests { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
