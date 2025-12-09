using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record BashFileOptions([property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string FilePath) : BashOptions;