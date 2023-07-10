using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("buildx bake")]
public record DockerBuildxBakeOptions : DockerOptions
{
    [CommandLongSwitch("load")]
    public string? Load { get; set; }

    [CommandLongSwitch("metadata-file")]
    public string? MetadataFile { get; set; }

    [CommandLongSwitch("push")]
    public string? Push { get; set; }

}