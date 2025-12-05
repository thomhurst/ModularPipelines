using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "export")]
public record GcloudDnsRecordSetsExportOptions(
[property: CliArgument] string RecordsFile,
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--zone-file-format")]
    public bool? ZoneFileFormat { get; set; }
}