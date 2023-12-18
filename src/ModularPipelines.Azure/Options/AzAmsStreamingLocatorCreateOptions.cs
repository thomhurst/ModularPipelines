using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "streaming-locator", "create")]
public record AzAmsStreamingLocatorCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--asset-name")] string AssetName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--streaming-policy-name")] string StreamingPolicyName
) : AzOptions
{
    [CommandSwitch("--alternative-media-id")]
    public string? AlternativeMediaId { get; set; }

    [CommandSwitch("--content-key-policy-name")]
    public string? ContentKeyPolicyName { get; set; }

    [CommandSwitch("--content-keys")]
    public string? ContentKeys { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--streaming-locator-id")]
    public string? StreamingLocatorId { get; set; }
}