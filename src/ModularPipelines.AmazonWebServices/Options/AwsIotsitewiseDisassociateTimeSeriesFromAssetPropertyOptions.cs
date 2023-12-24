using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "disassociate-time-series-from-asset-property")]
public record AwsIotsitewiseDisassociateTimeSeriesFromAssetPropertyOptions(
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