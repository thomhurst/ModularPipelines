using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-deployments", "resume")]
public record GcloudComputeOsConfigPatchDeploymentsResumeOptions(
[property: PositionalArgument] string PatchDeployment
) : GcloudOptions;