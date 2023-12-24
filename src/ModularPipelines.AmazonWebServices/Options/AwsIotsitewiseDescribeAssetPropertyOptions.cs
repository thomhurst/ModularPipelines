using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "describe-asset-property")]
public record AwsIotsitewiseDescribeAssetPropertyOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--property-id")] string PropertyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}