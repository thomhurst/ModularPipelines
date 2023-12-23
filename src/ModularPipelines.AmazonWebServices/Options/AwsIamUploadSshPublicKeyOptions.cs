using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "upload-ssh-public-key")]
public record AwsIamUploadSshPublicKeyOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--ssh-public-key-body")] string SshPublicKeyBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}