using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "create-user-profile")]
public record AwsOpsworksCreateUserProfileOptions(
[property: CliOption("--iam-user-arn")] string IamUserArn
) : AwsOptions
{
    [CliOption("--ssh-username")]
    public string? SshUsername { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}