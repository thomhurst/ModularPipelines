using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-sync", "get-identity-pool-configuration")]
public record AwsCognitoSyncGetIdentityPoolConfigurationOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}