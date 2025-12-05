using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "delete-ssh-public-key")]
public record AwsIamDeleteSshPublicKeyOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--ssh-public-key-id")] string SshPublicKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}