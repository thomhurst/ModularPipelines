using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kafka", "batch-disassociate-scram-secret")]
public record AwsKafkaBatchDisassociateScramSecretOptions(
[property: CliOption("--cluster-arn")] string ClusterArn,
[property: CliOption("--secret-arn-list")] string[] SecretArnList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}