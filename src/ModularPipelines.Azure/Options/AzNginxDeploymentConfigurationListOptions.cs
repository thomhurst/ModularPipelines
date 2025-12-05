using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("nginx", "deployment", "configuration", "list")]
public record AzNginxDeploymentConfigurationListOptions(
[property: CliOption("--deployment-name")] string DeploymentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;