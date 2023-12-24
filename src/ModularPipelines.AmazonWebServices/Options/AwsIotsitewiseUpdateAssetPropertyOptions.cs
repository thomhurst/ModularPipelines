using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotsitewise", "update-asset-property")]
public record AwsIotsitewiseUpdateAssetPropertyOptions(
[property: CommandSwitch("--asset-id")] string AssetId,
[property: CommandSwitch("--property-id")] string PropertyId
) : AwsOptions
{
    [CommandSwitch("--property-alias")]
    public string? PropertyAlias { get; set; }

    [CommandSwitch("--property-notification-state")]
    public string? PropertyNotificationState { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--property-unit")]
    public string? PropertyUnit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}