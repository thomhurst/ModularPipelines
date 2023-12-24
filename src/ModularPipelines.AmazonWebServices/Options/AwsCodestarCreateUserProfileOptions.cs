using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar", "create-user-profile")]
public record AwsCodestarCreateUserProfileOptions(
[property: CommandSwitch("--user-arn")] string UserArn,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--email-address")] string EmailAddress
) : AwsOptions
{
    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}