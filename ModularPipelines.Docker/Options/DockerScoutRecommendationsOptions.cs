using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout recommendations")]
public record DockerScoutRecommendationsOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string? Image { get; set; }

    [CommandSwitch("--only-refresh")]
    public string? OnlyRefresh { get; set; }

    [CommandSwitch("--only-update")]
    public string? OnlyUpdate { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}