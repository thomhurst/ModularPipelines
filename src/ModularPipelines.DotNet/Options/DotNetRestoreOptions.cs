using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetRestoreOptions : DotNetOptions
{
    public DotNetRestoreOptions(
        string path
    )
    {
        CommandParts = ["restore", "[<ROOT>]"];

        Path = path;
    }

    public DotNetRestoreOptions()
    {
        CommandParts = ["restore", "[<ROOT>]"];
    }

    [PositionalArgument(PlaceholderName = "[<ROOT>]")]
    public string? Path { get; set; }

    [CommandSwitch("--configfile")]
    public virtual string? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-build-servers")]
    public virtual bool? DisableBuildServers { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--force-evaluate")]
    public virtual bool? ForceEvaluate { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--lock-file-path")]
    public virtual string? LockFilePath { get; set; }

    [BooleanCommandSwitch("--locked-mode")]
    public virtual bool? LockedMode { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [CommandSwitch("--packages")]
    public virtual string? PackagesDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CommandSwitch("--source")]
    public virtual IEnumerable<string>? Source { get; set; }

    [BooleanCommandSwitch("--tl")]
    public virtual bool? Tl { get; set; }

    [BooleanCommandSwitch("--use-current-runtime")]
    public virtual bool? UseCurrentRuntime { get; set; }

    [BooleanCommandSwitch("--use-lock-file")]
    public virtual bool? UseLockFile { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
