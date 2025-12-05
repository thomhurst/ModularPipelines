using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "describe")]
public record GcloudComputeOsConfigPatchDeploymentsDescribeOptions(
[property: CliArgument] string PatchDeployment
) : GcloudOptions;