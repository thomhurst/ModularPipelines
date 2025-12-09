using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "import-pipeline", "list")]
public record AzAcrImportPipelineListOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;