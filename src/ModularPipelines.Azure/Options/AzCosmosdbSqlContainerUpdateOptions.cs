using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cosmosdb", "sql", "container", "update")]
public record AzCosmosdbSqlContainerUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--analytical-storage-ttl")]
    public string? AnalyticalStorageTtl { get; set; }

    [CommandSwitch("--idx")]
    public string? Idx { get; set; }

    [CommandSwitch("--ttl")]
    public string? Ttl { get; set; }
}