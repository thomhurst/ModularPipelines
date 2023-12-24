using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "delete-asset-model-composite-model")]
public record AwsIotsitewiseDeleteAssetModelCompositeModelOptions(
[property: CommandSwitch("--asset-model-id")] string AssetModelId,
[property: CommandSwitch("--asset-model-composite-model-id")] string AssetModelCompositeModelId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}