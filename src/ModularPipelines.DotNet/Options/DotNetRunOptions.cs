using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[CliSubCommand("run")]
[ExcludeFromCodeCoverage]
public record DotNetRunOptions : DotNetOptions
{
    [CliOption("--arch")]
    public virtual string? Architecture { get; set; }

    [CliOption("--configuration")]
    public virtual string? Configuration { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliOption("--launch-profile")]
    public virtual string? LaunchProfile { get; set; }

    [CliFlag("--no-build")]
    public virtual bool? NoBuild { get; set; }

    [CliFlag("--no-dependencies")]
    public virtual bool? NoDependencies { get; set; }

    [CliFlag("--no-launch-profile")]
    public virtual bool? NoLaunchProfile { get; set; }

    [CliFlag("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CliOption("--os")]
    public virtual string? Os { get; set; }

    [CliOption("--project")]
    public virtual string? Project { get; set; }

    [CliOption("--runtime")]
    public virtual string? RuntimeIdentifier { get; set; }

    [CliFlag("--tl")]
    public virtual bool? Tl { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
