using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker-featurestore-runtime", "batch-get-record")]
public record AwsSagemakerFeaturestoreRuntimeBatchGetRecordOptions(
[property: CliOption("--identifiers")] string[] Identifiers
) : AwsOptions
{
    [CliOption("--expiration-time-response")]
    public string? ExpirationTimeResponse { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}