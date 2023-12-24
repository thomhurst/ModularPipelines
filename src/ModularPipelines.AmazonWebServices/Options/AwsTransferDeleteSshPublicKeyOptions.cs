using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "delete-ssh-public-key")]
public record AwsTransferDeleteSshPublicKeyOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--ssh-public-key-id")] string SshPublicKeyId,
[property: CommandSwitch("--user-name")] string UserName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}