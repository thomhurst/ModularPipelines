using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "get-user-attribute-verification-code")]
public record AwsCognitoIdpGetUserAttributeVerificationCodeOptions(
[property: CliOption("--access-token")] string AccessToken,
[property: CliOption("--attribute-name")] string AttributeName
) : AwsOptions
{
    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}