using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-authorizer")]
public record AwsApigatewayGetAuthorizerOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}