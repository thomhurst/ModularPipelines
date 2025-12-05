using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "waf-policy", "update")]
public record AzNetworkFrontDoorWafPolicyUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--custom-block-response-body")]
    public string? CustomBlockResponseBody { get; set; }

    [CliOption("--custom-block-response-status-code")]
    public string? CustomBlockResponseStatusCode { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--redirect-url")]
    public string? RedirectUrl { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--request-body-check")]
    public string? RequestBodyCheck { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}