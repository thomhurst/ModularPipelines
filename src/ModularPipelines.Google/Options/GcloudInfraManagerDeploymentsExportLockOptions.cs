using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("infra-manager", "deployments", "export-lock")]
public record GcloudInfraManagerDeploymentsExportLockOptions(
[property: PositionalArgument] string Deployment,
[property: PositionalArgument] string Location
) : GcloudOptions;