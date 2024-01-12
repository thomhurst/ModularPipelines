using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "dicom-stores", "export", "gcs")]
public record GcloudHealthcareDicomStoresExportGcsOptions(
[property: PositionalArgument] string DicomStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-uri-prefix")] string GcsUriPrefix
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--mime-type")]
    public string? MimeType { get; set; }
}