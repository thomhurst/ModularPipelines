using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("tag")]
public record DockerTagOptions(string SourceImage, string TargetImage) : DockerOptions;