using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "table", "show")]
public record AzDlaCatalogTableShowOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--schema-name")] string SchemaName,
[property: CommandSwitch("--table-name")] string TableName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}