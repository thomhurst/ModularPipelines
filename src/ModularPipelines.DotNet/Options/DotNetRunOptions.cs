using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CommandPrecedingArguments("run")]
[ExcludeFromCodeCoverage]
public record DotNetRunOptions : DotNetOptions
{
    [CommandSwitch("--arch")]
    public virtual string? Architecture { get; set; }

    [CommandSwitch("--configuration")]
    public virtual string? Configuration { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CommandSwitch("--launch-profile")]
    public virtual string? LaunchProfile { get; set; }

    [BooleanCommandSwitch("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [BooleanCommandSwitch("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [BooleanCommandSwitch("--no-launch-profile")]
    public virtual bool? NoLaunchProfile { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CommandSwitch("--os")]
    public virtual string? Os { get; set; }

    [CommandSwitch("--project")]
    public virtual string? Project { get; set; }

    [CommandSwitch("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [BooleanCommandSwitch("--tl")]
    public virtual bool? Tl { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
