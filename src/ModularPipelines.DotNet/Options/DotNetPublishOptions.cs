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
    public virtual string? Architecture { get; set; }

    [CommandSwitch("--configuration")]
    public virtual string? Configuration { get; set; }

    [BooleanCommandSwitch("--disable-build-servers")]
    public virtual bool? DisableBuildServers { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--manifest")]
    public virtual string? Manifest { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--nologo")]
    public virtual bool? Nologo { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CommandSwitch("--os")]
    public virtual string? Os { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--self-contained")]
    public virtual bool? SelfContained { get; set; }

    [BooleanCommandSwitch("--no-self-contained")]
    public virtual bool? NoSelfContained { get; set; }

    [CommandSwitch("--source")]
    public virtual string? Source { get; set; }

    [BooleanCommandSwitch("--tl")]
    public virtual bool? Tl { get; set; }

    [BooleanCommandSwitch("--use-current-runtime")]
    public virtual bool? UseCurrentRuntime { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CommandSwitch("--version-suffix")]
    public virtual string? VersionSuffix { get; set; }
}
