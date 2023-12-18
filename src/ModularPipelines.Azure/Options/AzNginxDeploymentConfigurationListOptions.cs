using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment", "configuration", "list")]
public record AzNginxDeploymentConfigurationListOptions(
[property: CommandSwitch("--deployment-name")] string DeploymentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;