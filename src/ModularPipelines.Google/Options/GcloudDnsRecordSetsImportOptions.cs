using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dns", "record-sets", "import")]
public record GcloudDnsRecordSetsImportOptions(
[property: CliArgument] string RecordsFile,
[property: CliOption("--zone")] string Zone
) : GcloudOptions
{
    [CliFlag("--delete-all-existing")]
    public bool? DeleteAllExisting { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--replace-origin-ns")]
    public bool? ReplaceOriginNs { get; set; }

    [CliFlag("--zone-file-format")]
    public bool? ZoneFileFormat { get; set; }
}