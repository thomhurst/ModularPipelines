using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "install-status")]
public record GcloudWorkspaceAddOnsDeploymentsInstallStatusOptions(
[property: PositionalArgument] string Deployment
) : GcloudOptions;