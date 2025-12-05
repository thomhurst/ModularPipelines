using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "hl7v2-stores", "create")]
public record GcloudHealthcareHl7v2StoresCreateOptions(
[property: CliArgument] string HL7V2Store,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--notification-config")]
    public string[]? NotificationConfig { get; set; }

    [CliOption("--parser-version")]
    public string? ParserVersion { get; set; }
}