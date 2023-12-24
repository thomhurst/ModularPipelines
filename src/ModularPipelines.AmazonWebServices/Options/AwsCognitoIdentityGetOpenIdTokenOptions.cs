using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "get-open-id-token")]
public record AwsCognitoIdentityGetOpenIdTokenOptions(
[property: CommandSwitch("--identity-id")] string IdentityId
) : AwsOptions
{
    [CommandSwitch("--logins")]
    public IEnumerable<KeyValue>? Logins { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}