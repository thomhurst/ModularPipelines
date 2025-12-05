using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-request-validator")]
public record AwsApigatewayGetRequestValidatorOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--request-validator-id")] string RequestValidatorId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}