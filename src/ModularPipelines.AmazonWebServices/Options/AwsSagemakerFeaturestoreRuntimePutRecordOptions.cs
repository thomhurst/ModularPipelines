using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-featurestore-runtime", "put-record")]
public record AwsSagemakerFeaturestoreRuntimePutRecordOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName,
[property: CliOption("--record")] string[] Record
) : AwsOptions
{
    [CliOption("--target-stores")]
    public string[]? TargetStores { get; set; }

    [CliOption("--ttl-duration")]
    public string? TtlDuration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}