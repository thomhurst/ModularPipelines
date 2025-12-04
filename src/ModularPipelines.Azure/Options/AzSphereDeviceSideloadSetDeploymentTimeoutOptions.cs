using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "sideload", "set-deployment-timeout")]
public record AzSphereDeviceSideloadSetDeploymentTimeoutOptions(
[property: CliOption("--value")] string Value
) : AzOptions;