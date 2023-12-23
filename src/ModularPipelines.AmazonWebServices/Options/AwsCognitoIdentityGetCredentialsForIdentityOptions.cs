using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-identity", "get-credentials-for-identity")]
public record AwsCognitoIdentityGetCredentialsForIdentityOptions(
[property: CommandSwitch("--identity-id")] string IdentityId
) : AwsOptions
{
    [CommandSwitch("--logins")]
    public IEnumerable<KeyValue>? Logins { get; set; }

    [CommandSwitch("--custom-role-arn")]
    public string? CustomRoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}