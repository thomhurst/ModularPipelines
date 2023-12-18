using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dla", "catalog", "table-type", "show")]
public record AzDlaCatalogTableTypeShowOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--schema-name")] string SchemaName,
[property: CommandSwitch("--table-type-name")] string TableTypeName
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}