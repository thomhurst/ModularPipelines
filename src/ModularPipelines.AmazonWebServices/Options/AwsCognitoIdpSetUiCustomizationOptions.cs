using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "set-ui-customization")]
public record AwsCognitoIdpSetUiCustomizationOptions(
[property: CliOption("--user-pool-id")] string UserPoolId
) : AwsOptions
{
    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--css")]
    public string? Css { get; set; }

    [CliOption("--image-file")]
    public string? ImageFile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}