using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cdn", "waf", "policy", "set")]
public record AzCdnWafPolicySetOptions : AzOptions
{
    [CommandSwitch("--block-response-body")]
    public string? BlockResponseBody { get; set; }

    [CommandSwitch("--block-response-status-code")]
    public string? BlockResponseStatusCode { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--redirect-url")]
    public string? RedirectUrl { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}