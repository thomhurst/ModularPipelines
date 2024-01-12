using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "simulate-maintenance-event")]
public record GcloudComputeInstancesSimulateMaintenanceEventOptions(
[property: PositionalArgument] string InstanceNames
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--with-extended-notifications")]
    public string? WithExtendedNotifications { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}