using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "alter-table-properties")]
public record GcloudMetastoreServicesAlterTablePropertiesOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--properties")] IEnumerable<KeyValue> Properties,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--update-mask")] string UpdateMask
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}