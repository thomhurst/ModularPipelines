using ModularPipelines.Attributes;

namespace ModularPipelines.Docker.Options;

[CommandPrecedingArguments("tag")]
public record DockerTagOptions([property: PositionalArgument] string SourceImage, [property: PositionalArgument] string TargetImage) : DockerOptions;