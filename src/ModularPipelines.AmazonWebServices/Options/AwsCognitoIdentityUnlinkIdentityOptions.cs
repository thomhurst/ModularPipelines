using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "unlink-identity")]
public record AwsCognitoIdentityUnlinkIdentityOptions(
[property: CommandSwitch("--identity-id")] string IdentityId,
[property: CommandSwitch("--logins")] IEnumerable<KeyValue> Logins,
[property: CommandSwitch("--logins-to-remove")] string[] LoginsToRemove
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}