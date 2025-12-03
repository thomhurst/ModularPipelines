using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "delete-user-attributes")]
public record AwsCognitoIdpDeleteUserAttributesOptions(
[property: CliOption("--user-attribute-names")] string[] UserAttributeNames,
[property: CliOption("--access-token")] string AccessToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}