using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connected-env", "dapr-component", "show")]
public record AzContainerappConnectedEnvDaprComponentShowOptions(
[property: CliOption("--dapr-component-name")] string DaprComponentName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;