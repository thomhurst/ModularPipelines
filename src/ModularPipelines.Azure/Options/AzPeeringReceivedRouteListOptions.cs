using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("peering", "received-route", "list")]
public record AzPeeringReceivedRouteListOptions(
[property: CliOption("--peering-name")] string PeeringName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--as-path")]
    public string? AsPath { get; set; }

    [CliOption("--origin-as-validation-state")]
    public string? OriginAsValidationState { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--rpki-validation-state")]
    public string? RpkiValidationState { get; set; }

    [CliOption("--skip-token")]
    public string? SkipToken { get; set; }
}