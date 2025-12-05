using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotsitewise", "update-asset-property")]
public record AwsIotsitewiseUpdateAssetPropertyOptions(
[property: CliOption("--asset-id")] string AssetId,
[property: CliOption("--property-id")] string PropertyId
) : AwsOptions
{
    [CliOption("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CliOption("--property-notification-state")]
    public string? PropertyNotificationState { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--property-unit")]
    public string? PropertyUnit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}