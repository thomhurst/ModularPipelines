using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

public record BashFileOptions([property: PositionalArgument(Position = Position.BeforeArguments)] string FilePath) : BashOptions;