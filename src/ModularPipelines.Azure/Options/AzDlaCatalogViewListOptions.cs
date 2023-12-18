using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "view", "list")]
public record AzDlaCatalogViewListOptions(
[property: CommandSwitch("--database-name")] string DatabaseName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--schema-name")]
    public string? SchemaName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}