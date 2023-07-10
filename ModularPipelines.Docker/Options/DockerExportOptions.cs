using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("export")]
public record DockerExportOptions : DockerOptions
{
    [CommandLongSwitch("output")]
    public string? Output { get; set; }

}