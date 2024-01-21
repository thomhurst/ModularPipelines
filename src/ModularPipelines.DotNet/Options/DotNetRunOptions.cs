using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("run")]
[ExcludeFromCodeCoverage]
public record DotNetRunOptions : DotNetOptions
{
    [CommandSwitch("--arch")]
    public string? Architecture { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public bool? Interactive { get; set; }

    [CommandSwitch("--launch-profile")]
    public string? LaunchProfile { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-launch-profile")]
    public bool? NoLaunchProfile { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [CommandSwitch("--os")]
    public string? Os { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [CommandSwitch("--runtime")]
    public string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--tl")]
    public bool? Tl { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
