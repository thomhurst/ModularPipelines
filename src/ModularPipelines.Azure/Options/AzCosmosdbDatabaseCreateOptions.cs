using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "database", "create")]
public record AzCosmosdbDatabaseCreateOptions(
[property: CommandSwitch("--db-name")] string DbName
) : AzOptions
{
    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group-name")]
    public string? ResourceGroupName { get; set; }

    [CommandSwitch("--throughput")]
    public string? Throughput { get; set; }

    [CommandSwitch("--url-connection")]
    public string? UrlConnection { get; set; }
}

