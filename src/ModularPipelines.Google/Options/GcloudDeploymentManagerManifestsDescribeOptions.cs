using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment-manager", "manifests", "describe")]
public record GcloudDeploymentManagerManifestsDescribeOptions(
[property: PositionalArgument] string Manifest,
[property: CommandSwitch("--deployment")] string Deployment
) : GcloudOptions;