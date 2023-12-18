using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "export-pipeline", "list")]
public record AzAcrExportPipelineListOptions(
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;