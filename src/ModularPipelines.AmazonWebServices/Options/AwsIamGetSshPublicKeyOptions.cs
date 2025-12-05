using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "get-ssh-public-key")]
public record AwsIamGetSshPublicKeyOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--ssh-public-key-id")] string SshPublicKeyId,
[property: CliOption("--encoding")] string Encoding
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}