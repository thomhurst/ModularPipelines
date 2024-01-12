using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "indexes", "fields", "update")]
public record GcloudFirestoreIndexesFieldsUpdateOptions(
[property: PositionalArgument] string Field,
[property: PositionalArgument] string CollectionGroup,
[property: PositionalArgument] string Database,
[property: BooleanCommandSwitch("--clear-exemption")] bool ClearExemption,
[property: BooleanCommandSwitch("--disable-indexes")] bool DisableIndexes,
[property: CommandSwitch("--index")] IEnumerable<KeyValue> Index,
[property: BooleanCommandSwitch("order")] bool Order,
[property: BooleanCommandSwitch("array-config")] bool ArrayConfig
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}