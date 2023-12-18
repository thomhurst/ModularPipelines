using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-application-server-instance", "stop")]
public record AzWorkloadsSapApplicationServerInstanceStopOptions : AzOptions
{
    [CommandSwitch("--application-instance-name")]
    public string? ApplicationInstanceName { get; set; }

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
    public string? Subscription { get; set; }
}