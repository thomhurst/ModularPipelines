using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "delete-ssh-public-key")]
public record AwsTransferDeleteSshPublicKeyOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--ssh-public-key-id")] string SshPublicKeyId,
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}