using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworks", "update-my-user-profile")]
public record AwsOpsworksUpdateMyUserProfileOptions : AwsOptions
{
    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}