using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "get-open-id-token-for-developer-identity")]
public record AwsCognitoIdentityGetOpenIdTokenForDeveloperIdentityOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--logins")] IEnumerable<KeyValue> Logins
) : AwsOptions
{
    [CommandSwitch("--identity-id")]
    public string? IdentityId { get; set; }

    [CommandSwitch("--principal-tags")]
    public IEnumerable<KeyValue>? PrincipalTags { get; set; }

    [CommandSwitch("--token-duration")]
    public long? TokenDuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}