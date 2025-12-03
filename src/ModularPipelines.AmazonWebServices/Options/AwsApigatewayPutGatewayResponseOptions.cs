using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "put-gateway-response")]
public record AwsApigatewayPutGatewayResponseOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--response-type")] string ResponseType
) : AwsOptions
{
    [CliOption("--status-code")]
    public string? StatusCode { get; set; }

    [CliOption("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CliOption("--response-templates")]
    public IEnumerable<KeyValue>? ResponseTemplates { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}