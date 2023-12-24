using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "get-user-attribute-verification-code")]
public record AwsCognitoIdpGetUserAttributeVerificationCodeOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CommandSwitch("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}