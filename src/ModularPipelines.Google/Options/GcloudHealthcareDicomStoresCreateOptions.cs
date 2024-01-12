using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "dicom-stores", "create")]
public record GcloudHealthcareDicomStoresCreateOptions(
[property: PositionalArgument] string DicomStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--pubsub-topic")]
    public string? PubsubTopic { get; set; }

    [BooleanCommandSwitch("--send-for-bulk-import")]
    public bool? SendForBulkImport { get; set; }

    [CommandSwitch("--stream-configs")]
    public string? StreamConfigs { get; set; }
}