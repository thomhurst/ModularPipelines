using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "test-invoke-authorizer")]
public record AwsApigatewayTestInvokeAuthorizerOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CliOption("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CliOption("--multi-value-headers")]
    public IEnumerable<KeyValue>? MultiValueHeaders { get; set; }

    [CliOption("--path-with-query-string")]
    public string? PathWithQueryString { get; set; }

    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--stage-variables")]
    public IEnumerable<KeyValue>? StageVariables { get; set; }

    [CliOption("--additional-context")]
    public IEnumerable<KeyValue>? AdditionalContext { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}