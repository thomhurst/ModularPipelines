using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "sideload", "set-deployment-timeout")]
public record AzSphereDeviceSideloadSetDeploymentTimeoutOptions(
[property: CommandSwitch("--value")] string Value
) : AzOptions;