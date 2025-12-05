using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "create-campaign")]
public record AwsIotfleetwiseCreateCampaignOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--signal-catalog-arn")] string SignalCatalogArn,
[property: CliOption("--target-arn")] string TargetArn,
[property: CliOption("--collection-scheme")] string CollectionScheme
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--expiry-time")]
    public long? ExpiryTime { get; set; }

    [CliOption("--post-trigger-collection-duration")]
    public long? PostTriggerCollectionDuration { get; set; }

    [CliOption("--diagnostics-mode")]
    public string? DiagnosticsMode { get; set; }

    [CliOption("--spooling-mode")]
    public string? SpoolingMode { get; set; }

    [CliOption("--compression")]
    public string? Compression { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--signals-to-collect")]
    public string[]? SignalsToCollect { get; set; }

    [CliOption("--data-extra-dimensions")]
    public string[]? DataExtraDimensions { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--data-destination-configs")]
    public string[]? DataDestinationConfigs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}