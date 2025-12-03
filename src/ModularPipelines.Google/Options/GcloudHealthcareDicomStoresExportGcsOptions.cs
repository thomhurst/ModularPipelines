using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "export", "gcs")]
public record GcloudHealthcareDicomStoresExportGcsOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--gcs-uri-prefix")] string GcsUriPrefix
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--mime-type")]
    public string? MimeType { get; set; }
}