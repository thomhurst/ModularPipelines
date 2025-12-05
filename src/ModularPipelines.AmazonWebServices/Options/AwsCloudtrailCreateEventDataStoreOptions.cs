using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "create-event-data-store")]
public record AwsCloudtrailCreateEventDataStoreOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--advanced-event-selectors")]
    public string[]? AdvancedEventSelectors { get; set; }

    [CliOption("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CliOption("--tags-list")]
    public string[]? TagsList { get; set; }

    [CliOption("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CliOption("--billing-mode")]
    public string? BillingMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}