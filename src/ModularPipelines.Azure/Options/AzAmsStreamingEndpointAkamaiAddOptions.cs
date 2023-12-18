using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "streaming-endpoint", "akamai", "add")]
public record AzAmsStreamingEndpointAkamaiAddOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--base64-key")]
    public string? Base64Key { get; set; }

    [CommandSwitch("--expiration")]
    public string? Expiration { get; set; }

    [CommandSwitch("--identifier")]
    public string? Identifier { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}