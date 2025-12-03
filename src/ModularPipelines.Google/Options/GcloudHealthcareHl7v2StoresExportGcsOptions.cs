using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "hl7v2-stores", "export", "gcs")]
public record GcloudHealthcareHl7v2StoresExportGcsOptions(
[property: CliArgument] string HL7V2Store,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location,
[property: CliOption("--gcs-uri")] string GcsUri
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--message-view")]
    public string? MessageView { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }
}