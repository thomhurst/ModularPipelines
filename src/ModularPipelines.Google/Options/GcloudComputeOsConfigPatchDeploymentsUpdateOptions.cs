using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-deployments", "update")]
public record GcloudComputeOsConfigPatchDeploymentsUpdateOptions(
[property: PositionalArgument] string PatchDeploymentId,
[property: CommandSwitch("--file")] string File
) : GcloudOptions;