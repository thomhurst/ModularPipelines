using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "deployments", "uninstall")]
public record GcloudWorkspaceAddOnsDeploymentsUninstallOptions(
[property: CliArgument] string Deployment
) : GcloudOptions;