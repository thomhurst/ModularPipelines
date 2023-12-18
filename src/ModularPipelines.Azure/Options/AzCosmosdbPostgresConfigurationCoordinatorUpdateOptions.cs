using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "postgres", "configuration", "coordinator", "update")]
public record AzCosmosdbPostgresConfigurationCoordinatorUpdateOptions : AzOptions
{
    [CommandSwitch("--cluster-name")]
    public string? ClusterName { get; set; }

    [CommandSwitch("--configuration-name")]
    public string? ConfigurationName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}