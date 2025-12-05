using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "verify-user-attribute")]
public record AwsCognitoIdpVerifyUserAttributeOptions(
[property: CliOption("--access-token")] string AccessToken,
[property: CliOption("--attribute-name")] string AttributeName,
[property: CliOption("--code")] string Code
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}