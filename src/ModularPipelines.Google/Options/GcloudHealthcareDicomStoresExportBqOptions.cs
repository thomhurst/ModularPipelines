using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "dicom-stores", "export", "bq")]
public record GcloudHealthcareDicomStoresExportBqOptions(
[property: CliArgument] string DicomStore,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--bq-table")] string BqTable
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--overwrite-table")]
    public bool? OverwriteTable { get; set; }

    [CliOption("--write-disposition")]
    public string? WriteDisposition { get; set; }
}