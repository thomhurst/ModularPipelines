using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "associate-software-token")]
public record AwsCognitoIdpAssociateSoftwareTokenOptions : AwsOptions
{
    [CliOption("--access-token")]
    public string? AccessToken { get; set; }

    [CliOption("--session")]
    public string? Session { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}