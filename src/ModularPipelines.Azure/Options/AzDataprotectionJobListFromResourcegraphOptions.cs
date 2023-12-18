using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "job", "list-from-resourcegraph")]
public record AzDataprotectionJobListFromResourcegraphOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType
) : AzOptions
{
    [CommandSwitch("--datasource-id")]
    public string? DatasourceId { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--operation")]
    public string? Operation { get; set; }

    [CommandSwitch("--resource-groups")]
    public string? ResourceGroups { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CommandSwitch("--vaults")]
    public string? Vaults { get; set; }
}

