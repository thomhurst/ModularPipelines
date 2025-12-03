using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-repository-encryption-key")]
public record AwsCodecommitUpdateRepositoryEncryptionKeyOptions(
[property: CliOption("--repository-name")] string RepositoryName,
[property: CliOption("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}