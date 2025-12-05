using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-update-user-attributes")]
public record AwsCognitoIdpAdminUpdateUserAttributesOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username,
[property: CliOption("--user-attributes")] string[] UserAttributes
) : AwsOptions
{
    [CliOption("--client-metadata")]
    public IEnumerable<KeyValue>? ClientMetadata { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}