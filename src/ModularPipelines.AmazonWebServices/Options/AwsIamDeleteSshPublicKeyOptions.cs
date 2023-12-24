using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "delete-ssh-public-key")]
public record AwsIamDeleteSshPublicKeyOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--ssh-public-key-id")] string SshPublicKeyId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}