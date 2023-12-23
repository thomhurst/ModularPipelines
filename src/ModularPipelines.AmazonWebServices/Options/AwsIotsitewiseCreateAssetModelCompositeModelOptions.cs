using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "create-asset-model-composite-model")]
public record AwsIotsitewiseCreateAssetModelCompositeModelOptions(
[property: CommandSwitch("--asset-model-id")] string AssetModelId,
[property: CommandSwitch("--asset-model-composite-model-name")] string AssetModelCompositeModelName,
[property: CommandSwitch("--asset-model-composite-model-type")] string AssetModelCompositeModelType
) : AwsOptions
{
    [CommandSwitch("--parent-asset-model-composite-model-id")]
    public string? ParentAssetModelCompositeModelId { get; set; }

    [CommandSwitch("--asset-model-composite-model-external-id")]
    public string? AssetModelCompositeModelExternalId { get; set; }

    [CommandSwitch("--asset-model-composite-model-id")]
    public string? AssetModelCompositeModelId { get; set; }

    [CommandSwitch("--asset-model-composite-model-description")]
    public string? AssetModelCompositeModelDescription { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--composed-asset-model-id")]
    public string? ComposedAssetModelId { get; set; }

    [CommandSwitch("--asset-model-composite-model-properties")]
    public string[]? AssetModelCompositeModelProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}