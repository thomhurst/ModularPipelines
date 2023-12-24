using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "import-ssh-public-key")]
public record AwsTransferImportSshPublicKeyOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--ssh-public-key-body")] string SshPublicKeyBody,
[property: CommandSwitch("--user-name")] string UserName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}