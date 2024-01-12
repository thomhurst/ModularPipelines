using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "uninstall")]
public record GcloudWorkspaceAddOnsDeploymentsUninstallOptions(
[property: PositionalArgument] string Deployment
) : GcloudOptions;