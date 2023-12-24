using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "create-campaign")]
public record AwsIotfleetwiseCreateCampaignOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--signal-catalog-arn")] string SignalCatalogArn,
[property: CommandSwitch("--target-arn")] string TargetArn,
[property: CommandSwitch("--collection-scheme")] string CollectionScheme
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--expiry-time")]
    public long? ExpiryTime { get; set; }

    [CommandSwitch("--post-trigger-collection-duration")]
    public long? PostTriggerCollectionDuration { get; set; }

    [CommandSwitch("--diagnostics-mode")]
    public string? DiagnosticsMode { get; set; }

    [CommandSwitch("--spooling-mode")]
    public string? SpoolingMode { get; set; }

    [CommandSwitch("--compression")]
    public string? Compression { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--signals-to-collect")]
    public string[]? SignalsToCollect { get; set; }

    [CommandSwitch("--data-extra-dimensions")]
    public string[]? DataExtraDimensions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--data-destination-configs")]
    public string[]? DataDestinationConfigs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}