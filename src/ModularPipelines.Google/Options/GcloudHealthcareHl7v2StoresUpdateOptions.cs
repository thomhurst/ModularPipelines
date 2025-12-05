using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthcare", "hl7v2-stores", "update")]
public record GcloudHealthcareHl7v2StoresUpdateOptions(
[property: CliArgument] string HL7V2Store,
[property: CliArgument] string Dataset,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--notification-config")]
    public string[]? NotificationConfig { get; set; }
}