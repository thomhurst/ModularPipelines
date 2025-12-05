using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-ssh-public-key")]
public record AwsIamUpdateSshPublicKeyOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--ssh-public-key-id")] string SshPublicKeyId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}