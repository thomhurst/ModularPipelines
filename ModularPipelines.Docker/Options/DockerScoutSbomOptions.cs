using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("scout sbom")]
public record DockerScoutSbomOptions : DockerOptions
{
    [CommandLongSwitch("format")]
    public string? Format { get; set; }

    [CommandLongSwitch("only-package-type")]
    public string? OnlyPackageType { get; set; }

    [CommandLongSwitch("output")]
    public string? Output { get; set; }

    [CommandLongSwitch("platform")]
    public string? Platform { get; set; }

    [CommandLongSwitch("ref")]
    public string? Ref { get; set; }

    [CommandLongSwitch("type")]
    public string? Type { get; set; }

}