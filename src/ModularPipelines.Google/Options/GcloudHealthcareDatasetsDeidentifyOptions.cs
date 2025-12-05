using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "datasets", "deidentify")]
public record GcloudHealthcareDatasetsDeidentifyOptions(
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--destination-dataset")] string DestinationDataset
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--default-fhir-config")]
    public bool? DefaultFhirConfig { get; set; }

    [CliOption("--dicom-filter-tags")]
    public string[]? DicomFilterTags { get; set; }

    [CliOption("--text-redaction-mode")]
    public string? TextRedactionMode { get; set; }
}