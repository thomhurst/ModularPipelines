using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nginx", "deployment", "certificate", "list")]
public record AzNginxDeploymentCertificateListOptions(
[property: CliOption("--deployment-name")] string DeploymentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;