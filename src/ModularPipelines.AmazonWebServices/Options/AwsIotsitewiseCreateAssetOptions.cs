using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-asset")]
public record AwsIotsitewiseCreateAssetOptions(
[property: CommandSwitch("--asset-name")] string AssetName,
[property: CommandSwitch("--asset-model-id")] string AssetModelId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--asset-description")]
    public string? AssetDescription { get; set; }

    [CommandSwitch("--asset-id")]
    public string? AssetId { get; set; }

    [CommandSwitch("--asset-external-id")]
    public string? AssetExternalId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}