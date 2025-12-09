using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "import-pipeline", "delete")]
public record AzAcrImportPipelineDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;