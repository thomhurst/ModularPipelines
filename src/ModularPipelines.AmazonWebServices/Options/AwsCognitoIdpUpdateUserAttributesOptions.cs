using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "update-user-attributes")]
public record AwsCognitoIdpUpdateUserAttributesOptions(
[property: CliOption("--user-attributes")] string[] UserAttributes,
[property: CliOption("--access-token")] string AccessToken
) : AwsOptions
{
    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}