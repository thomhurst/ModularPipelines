using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record BashFileOptions([property: PositionalArgument(Position = Position.BeforeArguments)] string FilePath) : BashOptions;
