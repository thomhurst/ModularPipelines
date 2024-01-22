using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "watch")]
[ExcludeFromCodeCoverage]
public record DockerScoutWatchOptions : DockerOptions
{
    [CommandSwitch("--all-images")]
    public string? AllImages { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--interval")]
    public int? Interval { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--refresh-registry")]
    public string? RefreshRegistry { get; set; }

    [CommandSwitch("--registry")]
    public string? Registry { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [BooleanCommandSwitch("--sbom")]
    public bool? Sbom { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--workers")]
    public int? Workers { get; set; }
}
