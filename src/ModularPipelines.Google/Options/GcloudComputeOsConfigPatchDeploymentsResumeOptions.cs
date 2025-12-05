using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "resume")]
public record GcloudComputeOsConfigPatchDeploymentsResumeOptions(
[property: CliArgument] string PatchDeployment
) : GcloudOptions;