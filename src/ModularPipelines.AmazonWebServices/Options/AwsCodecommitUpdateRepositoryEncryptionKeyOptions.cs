using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-repository-encryption-key")]
public record AwsCodecommitUpdateRepositoryEncryptionKeyOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--kms-key-id")] string KmsKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}