using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "create-feature-group")]
public record AwsSagemakerCreateFeatureGroupOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName,
[property: CliOption("--record-identifier-feature-name")] string RecordIdentifierFeatureName,
[property: CliOption("--event-time-feature-name")] string EventTimeFeatureName,
[property: CliOption("--feature-definitions")] string[] FeatureDefinitions
) : AwsOptions
{
    [CliOption("--online-store-config")]
    public string? OnlineStoreConfig { get; set; }

    [CliOption("--offline-store-config")]
    public string? OfflineStoreConfig { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}