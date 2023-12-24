using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "verify-user-attribute")]
public record AwsCognitoIdpVerifyUserAttributeOptions(
[property: CommandSwitch("--access-token")] string AccessToken,
[property: CommandSwitch("--attribute-name")] string AttributeName,
[property: CommandSwitch("--code")] string Code
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}