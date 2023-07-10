using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout quickview")]
public record DockerScoutQuickviewOptions : DockerOptions
{
    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("ref")]
    public string? Ref { get; set; }

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}