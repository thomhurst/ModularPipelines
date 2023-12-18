using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "import-pipeline", "delete")]
public record AzAcrImportPipelineDeleteOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;