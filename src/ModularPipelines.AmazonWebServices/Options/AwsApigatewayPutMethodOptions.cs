using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "put-method")]
public record AwsApigatewayPutMethodOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--http-method")] string HttpMethod,
[property: CliOption("--authorization-type")] string AuthorizationType
) : AwsOptions
{
    [CliOption("--authorizer-id")]
    public string? AuthorizerId { get; set; }

    [CliOption("--operation-name")]
    public string? OperationName { get; set; }

    [CliOption("--request-parameters")]
    public IEnumerable<KeyValue>? RequestParameters { get; set; }

    [CliOption("--request-models")]
    public IEnumerable<KeyValue>? RequestModels { get; set; }

    [CliOption("--request-validator-id")]
    public string? RequestValidatorId { get; set; }

    [CliOption("--authorization-scopes")]
    public string[]? AuthorizationScopes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}