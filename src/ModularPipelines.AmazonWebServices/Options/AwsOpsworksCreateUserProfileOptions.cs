using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworks", "create-user-profile")]
public record AwsOpsworksCreateUserProfileOptions(
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