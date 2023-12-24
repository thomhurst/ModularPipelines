using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "update-user-profile")]
public record AwsOpsworksUpdateUserProfileOptions(
[property: CommandSwitch("--iam-user-arn")] string IamUserArn
) : AwsOptions
{
    [CommandSwitch("--ssh-username")]
    public string? SshUsername { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}