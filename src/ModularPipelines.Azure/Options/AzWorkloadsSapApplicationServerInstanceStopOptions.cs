using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "sap-application-server-instance", "stop")]
public record AzWorkloadsSapApplicationServerInstanceStopOptions : AzOptions
{
    [CliOption("--application-instance-name")]
    public string? ApplicationInstanceName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sap-virtual-instance-name")]
    public string? SapVirtualInstanceName { get; set; }

    [CliOption("--soft-stop-timeout-seconds")]
    public string? SoftStopTimeoutSeconds { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}