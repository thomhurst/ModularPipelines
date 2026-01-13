using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public partial record BashFileOptions([property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string FilePath) : BashOptions;