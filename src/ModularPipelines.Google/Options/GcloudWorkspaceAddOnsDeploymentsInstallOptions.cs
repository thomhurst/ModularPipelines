using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspace-add-ons", "deployments", "install")]
public record GcloudWorkspaceAddOnsDeploymentsInstallOptions(
[property: PositionalArgument] string Deployment
) : GcloudOptions;