using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("infra-manager", "deployments", "export-lock")]
public record GcloudInfraManagerDeploymentsExportLockOptions(
[property: CliArgument] string Deployment,
[property: CliArgument] string Location
) : GcloudOptions;