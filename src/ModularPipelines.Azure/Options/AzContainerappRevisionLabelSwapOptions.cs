using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "revision", "label", "swap")]
public record AzContainerappRevisionLabelSwapOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source")] string Source,
[property: CliOption("--target")] string Target
) : AzOptions;