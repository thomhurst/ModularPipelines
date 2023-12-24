using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "get-id")]
public record AwsCognitoIdentityGetIdOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId
) : AwsOptions
{
    [CommandSwitch("--account-id")]
    public string? AccountId { get; set; }

    [CommandSwitch("--logins")]
    public IEnumerable<KeyValue>? Logins { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}