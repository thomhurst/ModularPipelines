using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "import-ssh-public-key")]
public record AwsTransferImportSshPublicKeyOptions(
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--ssh-public-key-body")] string SshPublicKeyBody,
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}