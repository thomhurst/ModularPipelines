using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "create")]
public record GcloudComputeOsConfigPatchDeploymentsCreateOptions(
[property: CliArgument] string PatchDeploymentId,
[property: CliOption("--file")] string File
) : GcloudOptions;