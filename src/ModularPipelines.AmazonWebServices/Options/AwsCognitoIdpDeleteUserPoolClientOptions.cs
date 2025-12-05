using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "delete-user-pool-client")]
public record AwsCognitoIdpDeleteUserPoolClientOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--client-id")] string ClientId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}