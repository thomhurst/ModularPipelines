using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firestore", "indexes", "fields", "update")]
public record GcloudFirestoreIndexesFieldsUpdateOptions(
[property: CliArgument] string Field,
[property: CliArgument] string CollectionGroup,
[property: CliArgument] string Database,
[property: CliFlag("--clear-exemption")] bool ClearExemption,
[property: CliFlag("--disable-indexes")] bool DisableIndexes,
[property: CliOption("--index")] IEnumerable<KeyValue> Index,
[property: CliFlag("order")] bool Order,
[property: CliFlag("array-config")] bool ArrayConfig
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}