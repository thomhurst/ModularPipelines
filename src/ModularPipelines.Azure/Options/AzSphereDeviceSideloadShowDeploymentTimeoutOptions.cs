using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "sideload", "show-deployment-timeout")]
public record AzSphereDeviceSideloadShowDeploymentTimeoutOptions : AzOptions;