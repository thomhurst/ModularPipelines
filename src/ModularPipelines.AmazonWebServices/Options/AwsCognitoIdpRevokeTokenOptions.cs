using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "revoke-token")]
public record AwsCognitoIdpRevokeTokenOptions(
[property: CliOption("--token")] string Token,
[property: CliOption("--client-id")] string ClientId
) : AwsOptions
{
    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}