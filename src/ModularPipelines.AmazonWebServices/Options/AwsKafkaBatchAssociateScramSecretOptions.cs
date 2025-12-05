using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "batch-associate-scram-secret")]
public record AwsKafkaBatchAssociateScramSecretOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--secret-arn-list")] string[] SecretArnList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}