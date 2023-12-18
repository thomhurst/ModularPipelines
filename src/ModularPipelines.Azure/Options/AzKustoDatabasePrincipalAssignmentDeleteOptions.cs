using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kusto", "database-principal-assignment", "delete")]
public record AzKustoDatabasePrincipalAssignmentDeleteOptions : AzOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--principal-assignment-name")]
    public string? PrincipalAssignmentName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}