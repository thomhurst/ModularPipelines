using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar", "update-user-profile")]
public record AwsCodestarUpdateUserProfileOptions(
[property: CliOption("--user-arn")] string UserArn
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--email-address")]
    public string? EmailAddress { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}