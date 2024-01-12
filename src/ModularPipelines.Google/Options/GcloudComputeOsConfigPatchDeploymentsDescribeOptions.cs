using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-deployments", "describe")]
public record GcloudComputeOsConfigPatchDeploymentsDescribeOptions(
[property: PositionalArgument] string PatchDeployment
) : GcloudOptions;