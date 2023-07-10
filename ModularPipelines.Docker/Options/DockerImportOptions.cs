using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("import")]
public record DockerImportOptions : DockerOptions
{
    [CommandLongSwitch("change")]
    public string? Change { get; set; }

    [CommandLongSwitch("message")]
    public string? Message { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

}