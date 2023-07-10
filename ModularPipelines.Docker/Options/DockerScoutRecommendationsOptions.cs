using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout recommendations")]
public record DockerScoutRecommendationsOptions : DockerOptions
{
    [CommandLongSwitch("only-refresh")]
    public string? OnlyRefresh { get; set; }

    [CommandLongSwitch("only-update")]
    public string? OnlyUpdate { get; set; }

    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("ref")]
    public string? Ref { get; set; }

    [CommandLongSwitch("tag")]
    public string? Tag { get; set; }

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}