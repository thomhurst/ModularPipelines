using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "trigger", "update")]
public record AzCosmosdbSqlTriggerUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--operation")]
    public string? Operation { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}

