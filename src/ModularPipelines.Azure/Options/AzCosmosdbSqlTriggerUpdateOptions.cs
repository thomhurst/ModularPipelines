using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cosmosdb", "sql", "trigger", "update")]
public record AzCosmosdbSqlTriggerUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--operation")]
    public string? Operation { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}