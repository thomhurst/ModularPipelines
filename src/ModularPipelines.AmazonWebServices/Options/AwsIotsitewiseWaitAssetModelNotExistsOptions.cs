using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "wait", "asset-model-not-exists")]
public record AwsIotsitewiseWaitAssetModelNotExistsOptions(
[property: CommandSwitch("--asset-model-id")] string AssetModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}