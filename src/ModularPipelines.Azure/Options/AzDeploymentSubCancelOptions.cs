using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deployment", "sub", "cancel")]
public record AzDeploymentSubCancelOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;