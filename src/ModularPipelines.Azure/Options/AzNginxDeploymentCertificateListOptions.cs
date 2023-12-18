using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nginx", "deployment", "certificate", "list")]
public record AzNginxDeploymentCertificateListOptions(
[property: CommandSwitch("--deployment-name")] string DeploymentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;