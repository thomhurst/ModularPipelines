using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "env", "dapr-component", "resiliency", "list")]
public record AzContainerappEnvDaprComponentResiliencyListOptions(
[property: CommandSwitch("--dapr-component-name")] string DaprComponentName,
[property: CommandSwitch("--environment")] string Environment,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;