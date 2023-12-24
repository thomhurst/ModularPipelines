using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "create-event-data-store")]
public record AwsCloudtrailCreateEventDataStoreOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--advanced-event-selectors")]
    public string[]? AdvancedEventSelectors { get; set; }

    [CommandSwitch("--retention-period")]
    public int? RetentionPeriod { get; set; }

    [CommandSwitch("--tags-list")]
    public string[]? TagsList { get; set; }

    [CommandSwitch("--kms-key-id")]
    public string? KmsKeyId { get; set; }

    [CommandSwitch("--billing-mode")]
    public string? BillingMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}