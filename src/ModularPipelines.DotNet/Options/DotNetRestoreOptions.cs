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
    public string? Configfile { get; set; }

    [BooleanCommandSwitch("--disable-build-servers")]
    public bool? DisableBuildServers { get; set; }

    [BooleanCommandSwitch("--disable-parallel")]
    public bool? DisableParallel { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--force-evaluate")]
    public bool? ForceEvaluate { get; set; }

    [BooleanCommandSwitch("--ignore-failed-sources")]
    public bool? IgnoreFailedSources { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--lock-file-path")]
    public string? LockFilePath { get; set; }

    [BooleanCommandSwitch("--locked-mode")]
    public bool? LockedMode { get; set; }

    [BooleanCommandSwitch("--no-cache")]
    public bool? NoCache { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; set; }

    [CommandSwitch("--packages")]
    public string? PackagesDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [CommandSwitch("--source")]
    public IEnumerable<string>? Source { get; set; }

    [BooleanCommandSwitch("--tl")]
    public bool? Tl { get; set; }

    [BooleanCommandSwitch("--use-current-runtime")]
    public bool? UseCurrentRuntime { get; set; }

    [BooleanCommandSwitch("--use-lock-file")]
    public bool? UseLockFile { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
