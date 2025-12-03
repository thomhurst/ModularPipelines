using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-featurestore-runtime", "delete-record")]
public record AwsSagemakerFeaturestoreRuntimeDeleteRecordOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName,
[property: CliOption("--record-identifier-value-as-string")] string RecordIdentifierValueAsString,
[property: CliOption("--event-time")] string EventTime
) : AwsOptions
{
    [CliOption("--target-stores")]
    public string[]? TargetStores { get; set; }

    [CliOption("--deletion-mode")]
    public string? DeletionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}