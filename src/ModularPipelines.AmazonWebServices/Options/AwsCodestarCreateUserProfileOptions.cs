using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar", "create-user-profile")]
public record AwsCodestarCreateUserProfileOptions(
[property: CliOption("--user-arn")] string UserArn,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--email-address")] string EmailAddress
) : AwsOptions
{
    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}