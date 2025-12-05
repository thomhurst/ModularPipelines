using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "change-password")]
public record AwsCognitoIdpChangePasswordOptions(
[property: CliOption("--previous-password")] string PreviousPassword,
[property: CliOption("--proposed-password")] string ProposedPassword,
[property: CliOption("--access-token")] string AccessToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}