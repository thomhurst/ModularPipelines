using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "hl7v2-stores", "export", "gcs")]
public record GcloudHealthcareHl7v2StoresExportGcsOptions(
[property: PositionalArgument] string HL7V2Store,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--message-view")]
    public string? MessageView { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }
}