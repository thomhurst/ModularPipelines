using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ams", "streaming-endpoint", "akamai", "add")]
public record AzAmsStreamingEndpointAkamaiAddOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--base64-key")]
    public string? Base64Key { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--identifier")]
    public string? Identifier { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}