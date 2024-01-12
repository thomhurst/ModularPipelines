using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "import")]
public record GcloudDnsRecordSetsImportOptions(
[property: PositionalArgument] string RecordsFile,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [BooleanCommandSwitch("--delete-all-existing")]
    public bool? DeleteAllExisting { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--replace-origin-ns")]
    public bool? ReplaceOriginNs { get; set; }

    [BooleanCommandSwitch("--zone-file-format")]
    public bool? ZoneFileFormat { get; set; }
}