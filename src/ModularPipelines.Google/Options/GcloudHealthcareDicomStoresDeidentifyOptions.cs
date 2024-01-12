using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "dicom-stores", "deidentify")]
public record GcloudHealthcareDicomStoresDeidentifyOptions(
[property: PositionalArgument] string DicomStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--destination-store")] string DestinationStore
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--dicom-filter-tags")]
    public string[]? DicomFilterTags { get; set; }

    [CommandSwitch("--text-redaction-mode")]
    public string? TextRedactionMode { get; set; }
}