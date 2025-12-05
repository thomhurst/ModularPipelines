using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "add-custom-attributes")]
public record AwsCognitoIdpAddCustomAttributesOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--custom-attributes")] string[] CustomAttributes
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}