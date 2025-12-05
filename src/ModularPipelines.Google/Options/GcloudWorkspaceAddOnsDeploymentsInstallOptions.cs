using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workspace-add-ons", "deployments", "install")]
public record GcloudWorkspaceAddOnsDeploymentsInstallOptions(
[property: CliArgument] string Deployment
) : GcloudOptions;