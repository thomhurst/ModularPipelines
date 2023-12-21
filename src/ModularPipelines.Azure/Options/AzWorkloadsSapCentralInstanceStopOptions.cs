using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-central-instance", "stop")]
public record AzWorkloadsSapCentralInstanceStopOptions : AzOptions
{
    [CommandSwitch("--central-instance-name")]
    public string? CentralInstanceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sap-virtual-instance-name")]
    public string? SapVirtualInstanceName { get; set; }

    [CommandSwitch("--soft-stop-timeout-seconds")]
    public string? SoftStopTimeoutSeconds { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}