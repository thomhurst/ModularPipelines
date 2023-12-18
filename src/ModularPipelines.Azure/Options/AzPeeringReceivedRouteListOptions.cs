using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("peering", "received-route", "list")]
public record AzPeeringReceivedRouteListOptions(
[property: CommandSwitch("--peering-name")] string PeeringName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--as-path")]
    public string? AsPath { get; set; }

    [CommandSwitch("--origin-as-validation-state")]
    public string? OriginAsValidationState { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--rpki-validation-state")]
    public string? RpkiValidationState { get; set; }

    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}