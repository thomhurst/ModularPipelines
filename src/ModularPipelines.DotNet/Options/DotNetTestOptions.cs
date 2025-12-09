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

    [CliArgument(Name = "[<PROJECT> | <SOLUTION> | <DIRECTORY> | <DLL> | <EXE>]")]
    public virtual string? ProjectSolutionDirectoryDllExe { get; set; }

    [CliOption("--test-adapter-path")]
    public virtual string? TestAdapterPath { get; set; }

    [CliOption("--arch")]
    public virtual string? Architecture { get; set; }

    [CliFlag("--blame")]
    public virtual bool? Blame { get; set; }

    [CliFlag("--blame-crash")]
    public virtual bool? BlameCrash { get; set; }

    [CliOption("--blame-crash-dump-type")]
    public virtual string? BlameCrashDumpType { get; set; }

    [CliFlag("--blame-crash-collect-always")]
    public virtual bool? BlameCrashCollectAlways { get; set; }

    [CliFlag("--blame-hang")]
    public virtual bool? BlameHang { get; set; }

    [CliOption("--blame-hang-dump-type")]
    public virtual string? BlameHangDumpType { get; set; }

    [CliOption("--blame-hang-timeout")]
    public virtual string? BlameHangTimeout { get; set; }

    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    [CliOption("--collect")]
    public virtual string? Collect { get; set; }

    [CliOption("--diag")]
    public virtual string? Diag { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliOption("--environment")]
    public virtual IEnumerable<string>? Environment { get; set; }

    [CliOption("--filter")]
    public virtual string? Filter { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--logger")]
    public virtual IEnumerable<string>? Logger { get; set; }

    [CliFlag("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [CliFlag("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CliFlag("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliOption("--os")]
    public virtual string? Os { get; set; }

    [CliOption("--results-directory")]
    public virtual string? ResultsDirectory { get; set; }

    [CliOption("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliOption("--settings")]
    public virtual string? SettingsFile { get; set; }

    [CliFlag("--list-tests")]
    public virtual bool? ListTests { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
