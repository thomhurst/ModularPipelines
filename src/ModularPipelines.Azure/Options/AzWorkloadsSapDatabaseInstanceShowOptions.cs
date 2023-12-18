using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-database-instance", "show")]
public record AzWorkloadsSapDatabaseInstanceShowOptions : AzOptions
{
    [CommandSwitch("--database-instance-name")]
    public string? DatabaseInstanceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sap-virtual-instance-name")]
    public string? SapVirtualInstanceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}