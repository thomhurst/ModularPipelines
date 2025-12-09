using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "put-kms-encryption-key")]
public record AwsFrauddetectorPutKmsEncryptionKeyOptions(
[property: CliOption("--kms-encryption-key-arn")] string KmsEncryptionKeyArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}