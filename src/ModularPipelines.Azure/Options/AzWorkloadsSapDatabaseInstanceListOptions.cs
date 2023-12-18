using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-database-instance", "list")]
public record AzWorkloadsSapDatabaseInstanceListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sap-virtual-instance-name")] string SapVirtualInstanceName
) : AzOptions
{
    [CommandSwitch("--database-instance-name")]
    public string? DatabaseInstanceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

