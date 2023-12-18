using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workloads", "sap-application-server-instance", "list")]
public record AzWorkloadsSapApplicationServerInstanceListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sap-virtual-instance-name")] string SapVirtualInstanceName
) : AzOptions
{
    [CommandSwitch("--application-instance-name")]
    public string? ApplicationInstanceName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

