using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "update")]
public record GcloudComputeOsConfigPatchDeploymentsUpdateOptions(
[property: CliArgument] string PatchDeploymentId,
[property: CliOption("--file")] string File
) : GcloudOptions;