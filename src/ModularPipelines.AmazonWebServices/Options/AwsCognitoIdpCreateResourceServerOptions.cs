using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "create-resource-server")]
public record AwsCognitoIdpCreateResourceServerOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--identifier")] string Identifier,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--scopes")]
    public string[]? Scopes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}