using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "create")]
public record AzNetworkFrontDoorWafPolicyCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--custom-block-response-body")]
    public string? CustomBlockResponseBody { get; set; }

    [CliOption("--custom-block-response-status-code")]
    public string? CustomBlockResponseStatusCode { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--redirect-url")]
    public string? RedirectUrl { get; set; }

    [CliOption("--request-body-check")]
    public string? RequestBodyCheck { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}