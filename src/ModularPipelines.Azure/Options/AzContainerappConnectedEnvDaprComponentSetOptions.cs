using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connected-env", "dapr-component", "set")]
public record AzContainerappConnectedEnvDaprComponentSetOptions(
[property: CommandSwitch("--dapr-component-name")] string DaprComponentName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--yaml")] string Yaml
) : AzOptions
{
}