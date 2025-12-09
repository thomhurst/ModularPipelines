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

    [CliArgument(Name = "[<ROOT>]")]
    public virtual string? Path { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }

    [CliFlag("--disable-build-servers")]
    public virtual bool? DisableBuildServers { get; set; }

    [CliFlag("--disable-parallel")]
    public virtual bool? DisableParallel { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--force-evaluate")]
    public virtual bool? ForceEvaluate { get; set; }

    [CliFlag("--ignore-failed-sources")]
    public virtual bool? IgnoreFailedSources { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--lock-file-path")]
    public virtual string? LockFilePath { get; set; }

    [CliFlag("--locked-mode")]
    public virtual bool? LockedMode { get; set; }

    [CliFlag("--no-cache")]
    public virtual bool? NoCache { get; set; }

    [CliFlag("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [CliOption("--packages")]
    public virtual string? PackagesDirectory { get; set; }

    [CliOption("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliOption("--source")]
    public virtual IEnumerable<string>? Source { get; set; }

    [CliFlag("--tl")]
    public virtual bool? Tl { get; set; }

    [CliFlag("--use-current-runtime")]
    public virtual bool? UseCurrentRuntime { get; set; }

    [CliFlag("--use-lock-file")]
    public virtual bool? UseLockFile { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
