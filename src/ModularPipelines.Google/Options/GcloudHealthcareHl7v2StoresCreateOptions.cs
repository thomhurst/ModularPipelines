using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthcare", "hl7v2-stores", "create")]
public record GcloudHealthcareHl7v2StoresCreateOptions(
[property: PositionalArgument] string HL7V2Store,
[property: PositionalArgument] string Dataset,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--notification-config")]
    public string[]? NotificationConfig { get; set; }

    [CommandSwitch("--parser-version")]
    public string? ParserVersion { get; set; }
}