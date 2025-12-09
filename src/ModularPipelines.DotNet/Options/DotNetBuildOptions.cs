using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetBuildOptions : DotNetOptions
{
    public DotNetBuildOptions(
        string projectSolution
    )
    {
        CommandParts = ["build", "[<PROJECT>|<SOLUTION>]"];

        ProjectSolution = projectSolution;
    }

    public DotNetBuildOptions()
    {
        CommandParts = ["build", "[<PROJECT>|<SOLUTION>]"];
    }

    [CliArgument(Name = "[<PROJECT>|<SOLUTION>]")]
    public virtual string? ProjectSolution { get; set; }

    [CliOption("--arch")]
    public virtual string? Architecture { get; set; }

    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--disable-build-servers")]
    public virtual bool? DisableBuildServers { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliFlag("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [CliFlag("--no-incremental")]
    public virtual bool? NoIncremental { get; set; }

    [CliFlag("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CliFlag("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CliFlag("--no-self-contained")]
    public virtual bool? NoSelfContained { get; set; }

    [CliOption("--os")]
    public virtual string? Os { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliOption("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliFlag("--self-contained")]
    public virtual bool? SelfContained { get; set; }

    [CliOption("--source")]
    public virtual string? Source { get; set; }

    [CliFlag("--tl")]
    public virtual bool? Tl { get; set; }

    [CliFlag("--use-current-runtime")]
    public virtual bool? UseCurrentRuntime { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--version-suffix")]
    public virtual string? VersionSuffix { get; set; }
}
