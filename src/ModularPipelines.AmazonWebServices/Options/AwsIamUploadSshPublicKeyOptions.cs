using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "upload-ssh-public-key")]
public record AwsIamUploadSshPublicKeyOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--ssh-public-key-body")] string SshPublicKeyBody
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}