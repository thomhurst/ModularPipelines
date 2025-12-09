using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-featurestore-runtime", "get-record")]
public record AwsSagemakerFeaturestoreRuntimeGetRecordOptions(
[property: CliOption("--feature-group-name")] string FeatureGroupName,
[property: CliOption("--record-identifier-value-as-string")] string RecordIdentifierValueAsString
) : AwsOptions
{
    [CliOption("--feature-names")]
    public string[]? FeatureNames { get; set; }

    [CliOption("--expiration-time-response")]
    public string? ExpirationTimeResponse { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}