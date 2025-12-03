using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "update")]
public record GcloudHealthcareDicomStoresUpdateOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--pubsub-topic")]
    public string? PubsubTopic { get; set; }

    [CliFlag("--send-for-bulk-import")]
    public bool? SendForBulkImport { get; set; }

    [CliOption("--stream-configs")]
    public string? StreamConfigs { get; set; }
}