using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-sync", "set-identity-pool-configuration")]
public record AwsCognitoSyncSetIdentityPoolConfigurationOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CommandSwitch("--push-sync")]
    public string? PushSync { get; set; }

    [CommandSwitch("--cognito-streams")]
    public string? CognitoStreams { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}