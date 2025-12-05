using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-authorizer")]
public record AwsApigatewayDeleteAuthorizerOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}