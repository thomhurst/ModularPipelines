using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-deployments", "pause")]
public record GcloudComputeOsConfigPatchDeploymentsPauseOptions(
[property: CliArgument] string PatchDeployment
) : GcloudOptions;