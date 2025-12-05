using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-sync", "set-identity-pool-configuration")]
public record AwsCognitoSyncSetIdentityPoolConfigurationOptions(
[property: CliOption("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CliOption("--push-sync")]
    public string? PushSync { get; set; }

    [CliOption("--cognito-streams")]
    public string? CognitoStreams { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}