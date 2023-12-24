using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "associate-time-series-to-asset-property")]
public record AwsIotsitewiseAssociateTimeSeriesToAssetPropertyOptions(
[property: CommandSwitch("--alias")] string Alias,
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--property-id")] string PropertyId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}