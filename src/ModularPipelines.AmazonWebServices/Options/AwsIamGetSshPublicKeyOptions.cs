using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "get-ssh-public-key")]
public record AwsIamGetSshPublicKeyOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--ssh-public-key-id")] string SshPublicKeyId,
[property: CommandSwitch("--encoding")] string Encoding
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}