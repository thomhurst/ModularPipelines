using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "update-ssh-public-key")]
public record AwsIamUpdateSshPublicKeyOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--ssh-public-key-id")] string SshPublicKeyId,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}