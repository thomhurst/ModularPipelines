using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout quickview")]
public record DockerScoutQuickviewOptions : DockerOptions
{
    [PositionalArgument(Position = Position.AfterArguments)]
    public string Image { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--ref")]
    public string? Ref { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

}