using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "watch")]
[ExcludeFromCodeCoverage]
public record DockerScoutWatchOptions : DockerOptions
{
    [CommandSwitch("--all-images")]
    public virtual string? AllImages { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CommandSwitch("--interval")]
    public virtual int? Interval { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--refresh-registry")]
    public virtual string? RefreshRegistry { get; set; }

    [CommandSwitch("--registry")]
    public virtual string? Registry { get; set; }

    [CommandSwitch("--repository")]
    public virtual string? Repository { get; set; }

    [BooleanCommandSwitch("--sbom")]
    public virtual bool? Sbom { get; set; }

    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }

    [CommandSwitch("--workers")]
    public virtual int? Workers { get; set; }
}
