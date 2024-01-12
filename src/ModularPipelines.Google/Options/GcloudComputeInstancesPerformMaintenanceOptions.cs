using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "perform-maintenance")]
public record GcloudComputeInstancesPerformMaintenanceOptions(
[property: PositionalArgument] string InstanceName
) : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}