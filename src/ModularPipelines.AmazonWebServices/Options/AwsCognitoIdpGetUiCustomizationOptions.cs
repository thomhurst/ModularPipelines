using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "get-ui-customization")]
public record AwsCognitoIdpGetUiCustomizationOptions(
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}