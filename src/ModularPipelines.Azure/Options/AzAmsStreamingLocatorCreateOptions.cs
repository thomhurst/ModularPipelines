using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ams", "streaming-locator", "create")]
public record AzAmsStreamingLocatorCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--asset-name")] string AssetName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--streaming-policy-name")] string StreamingPolicyName
) : AzOptions
{
    [CliOption("--alternative-media-id")]
    public string? AlternativeMediaId { get; set; }

    [CliOption("--content-key-policy-name")]
    public string? ContentKeyPolicyName { get; set; }

    [CliOption("--content-keys")]
    public string? ContentKeys { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--streaming-locator-id")]
    public string? StreamingLocatorId { get; set; }
}