using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

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

    [PositionalArgument(PlaceholderName = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--arch")]
    public string? Architecture { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--disable-build-servers")]
    public bool? DisableBuildServers { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-incremental")]
    public bool? NoIncremental { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public bool? Nologo { get; set; }

    [BooleanCommandSwitch("--no-self-contained")]
    public bool? NoSelfContained { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--self-contained")]
    public bool? SelfContained { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--tl")]
    public bool? Tl { get; set; }

    [BooleanCommandSwitch("--use-current-runtime")]
    public bool? UseCurrentRuntime { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [CommandSwitch("--version-suffix")]
    public string? VersionSuffix { get; set; }
}
