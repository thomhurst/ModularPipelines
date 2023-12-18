using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "cluster-principal-assignment", "list")]
public record AzKustoClusterPrincipalAssignmentListOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--principal-assignment-name")]
    public string? PrincipalAssignmentName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}