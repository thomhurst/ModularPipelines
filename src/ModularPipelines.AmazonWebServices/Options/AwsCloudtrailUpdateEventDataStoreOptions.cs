using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "update-event-data-store")]
public record AwsCloudtrailUpdateEventDataStoreOptions(
[property: CommandSwitch("--event-data-store")] string EventDataStore
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--advanced-event-selectors")]
    public string[]? AdvancedEventSelectors { get; set; }

    [CommandSwitch("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--billing-mode")]
    public string? BillingMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}