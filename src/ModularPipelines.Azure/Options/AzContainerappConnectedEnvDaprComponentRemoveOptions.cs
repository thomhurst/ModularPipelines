using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connected-env", "dapr-component", "remove")]
public record AzContainerappConnectedEnvDaprComponentRemoveOptions(
[property: CommandSwitch("--dapr-component-name")] string DaprComponentName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;