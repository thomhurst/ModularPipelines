using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-asset-model-composite-model")]
public record AwsIotsitewiseUpdateAssetModelCompositeModelOptions(
[property: CommandSwitch("--asset-model-id")] string AssetModelId,
[property: CommandSwitch("--asset-model-composite-model-id")] string AssetModelCompositeModelId,
[property: CommandSwitch("--asset-model-composite-model-name")] string AssetModelCompositeModelName
) : AwsOptions
{
    [CommandSwitch("--asset-model-composite-model-external-id")]
    public string? AssetModelCompositeModelExternalId { get; set; }

    [CommandSwitch("--asset-model-composite-model-description")]
    public string? AssetModelCompositeModelDescription { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--asset-model-composite-model-properties")]
    public string[]? AssetModelCompositeModelProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}