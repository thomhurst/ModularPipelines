using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "export-pipeline", "list")]
public record AzAcrExportPipelineListOptions(
[property: CliOption("--registry")] string Registry,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;