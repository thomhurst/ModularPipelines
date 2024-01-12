using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-deployments", "delete")]
public record GcloudComputeOsConfigPatchDeploymentsDeleteOptions(
[property: PositionalArgument] string PatchDeployment
) : GcloudOptions;