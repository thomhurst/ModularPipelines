using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dns", "record-sets", "export")]
public record GcloudDnsRecordSetsExportOptions(
[property: PositionalArgument] string RecordsFile,
[property: CommandSwitch("--zone")] string Zone
) : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--zone-file-format")]
    public bool? ZoneFileFormat { get; set; }
}