using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "deployments", "install-status")]
public record GcloudWorkspaceAddOnsDeploymentsInstallStatusOptions(
[property: CliArgument] string Deployment
) : GcloudOptions;