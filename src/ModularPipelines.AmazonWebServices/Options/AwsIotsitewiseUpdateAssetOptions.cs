using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-asset")]
public record AwsIotsitewiseUpdateAssetOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--asset-name")] string AssetName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--asset-description")]
    public string? AssetDescription { get; set; }

    [CommandSwitch("--asset-external-id")]
    public string? AssetExternalId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}