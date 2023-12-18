using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "sideload", "set-deployment-timeout")]
public record AzSphereDeviceSideloadSetDeploymentTimeoutOptions(
[property: CommandSwitch("--value")] string Value
) : AzOptions
{
}