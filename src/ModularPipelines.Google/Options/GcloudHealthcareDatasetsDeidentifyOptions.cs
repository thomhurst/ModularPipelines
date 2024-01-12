using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "datasets", "deidentify")]
public record GcloudHealthcareDatasetsDeidentifyOptions(
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--destination-dataset")] string DestinationDataset
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--default-fhir-config")]
    public bool? DefaultFhirConfig { get; set; }

    [CommandSwitch("--dicom-filter-tags")]
    public string[]? DicomFilterTags { get; set; }

    [CommandSwitch("--text-redaction-mode")]
    public string? TextRedactionMode { get; set; }
}