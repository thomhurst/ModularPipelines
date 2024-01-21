using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetPublishOptions : DotNetOptions
{
    public DotNetPublishOptions(
        string projectSolution
    )
    {
        CommandParts = ["publish", "[<PROJECT>|<SOLUTION>]"];

        ProjectSolution = projectSolution;
    }

    public DotNetPublishOptions()
    {
        CommandParts = ["publish", "[<PROJECT>|<SOLUTION>]"];
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT>|<SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--arch")]
    public string? Architecture { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [BooleanCommandSwitch("--disable-build-servers")]
    public bool? DisableBuildServers { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--manifest")]
    public string? Manifest { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public bool? Nologo { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--self-contained")]
    public bool? SelfContained { get; set; }

    [BooleanCommandSwitch("--no-self-contained")]
    public bool? NoSelfContained { get; set; }

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
