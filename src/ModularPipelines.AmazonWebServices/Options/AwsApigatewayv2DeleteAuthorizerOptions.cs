using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "delete-authorizer")]
public record AwsApigatewayv2DeleteAuthorizerOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}