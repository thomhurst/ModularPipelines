using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "simulate-maintenance-event")]
public record GcloudComputeInstancesSimulateMaintenanceEventOptions(
[property: CliArgument] string InstanceNames
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--with-extended-notifications")]
    public string? WithExtendedNotifications { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}