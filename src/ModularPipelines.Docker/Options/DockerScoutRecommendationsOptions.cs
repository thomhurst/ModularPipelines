using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout", "recommendations")]
[ExcludeFromCodeCoverage]
public record DockerScoutRecommendationsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ImageOrDirectoryOrArchive { get; set; }

    [CommandSwitch("--only-refresh")]
    public virtual string? OnlyRefresh { get; set; }

    [CommandSwitch("--only-update")]
    public virtual string? OnlyUpdate { get; set; }

    [CommandSwitch("--org")]
    public virtual string? Org { get; set; }

    [CommandSwitch("--output")]
    public virtual string? Output { get; set; }

    [CommandSwitch("--platform")]
    public virtual string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public virtual string? Ref { get; set; }

    [CommandSwitch("--tag")]
    public virtual string? Tag { get; set; }
}
