using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "deidentify")]
public record GcloudHealthcareDicomStoresDeidentifyOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--destination-store")] string DestinationStore
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--dicom-filter-tags")]
    public string[]? DicomFilterTags { get; set; }

    [CliOption("--text-redaction-mode")]
    public string? TextRedactionMode { get; set; }
}