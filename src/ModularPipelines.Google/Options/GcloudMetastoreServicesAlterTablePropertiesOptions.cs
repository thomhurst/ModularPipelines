using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("metastore", "services", "alter-table-properties")]
public record GcloudMetastoreServicesAlterTablePropertiesOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location,
[property: CliOption("--properties")] IEnumerable<KeyValue> Properties,
[property: CliOption("--table-name")] string TableName,
[property: CliOption("--update-mask")] string UpdateMask
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}