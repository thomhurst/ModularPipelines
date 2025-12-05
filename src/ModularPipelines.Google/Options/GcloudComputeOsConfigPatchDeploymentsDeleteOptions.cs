using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "delete")]
public record GcloudComputeOsConfigPatchDeploymentsDeleteOptions(
[property: CliArgument] string PatchDeployment
) : GcloudOptions;