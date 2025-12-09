using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "test-invoke-method")]
public record AwsApigatewayTestInvokeMethodOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--http-method")] string HttpMethod
) : AwsOptions
{
    [CliOption("--path-with-query-string")]
    public string? PathWithQueryString { get; set; }

    [CliOption("--body")]
    public string? Body { get; set; }

    [CliOption("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CliOption("--multi-value-headers")]
    public IEnumerable<KeyValue>? MultiValueHeaders { get; set; }

    [CliOption("--client-certificate-id")]
    public string? ClientCertificateId { get; set; }

    [CliOption("--stage-variables")]
    public IEnumerable<KeyValue>? StageVariables { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}