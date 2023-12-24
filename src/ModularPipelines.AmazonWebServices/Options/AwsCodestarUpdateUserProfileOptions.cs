using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar", "update-user-profile")]
public record AwsCodestarUpdateUserProfileOptions(
[property: CommandSwitch("--user-arn")] string UserArn
) : AwsOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--email-address")]
    public string? EmailAddress { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}