using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "dicom-stores", "export", "bq")]
public record GcloudHealthcareDicomStoresExportBqOptions(
[property: PositionalArgument] string DicomStore,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--bq-table")] string BqTable
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--overwrite-table")]
    public bool? OverwriteTable { get; set; }

    [CommandSwitch("--write-disposition")]
    public string? WriteDisposition { get; set; }
}