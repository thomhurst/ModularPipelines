using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "put-integration-response")]
public record AwsApigatewayPutIntegrationResponseOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--http-method")] string HttpMethod,
[property: CliOption("--status-code")] string StatusCode
) : AwsOptions
{
    [CliOption("--selection-pattern")]
    public string? SelectionPattern { get; set; }

    [CliOption("--response-parameters")]
    public IEnumerable<KeyValue>? ResponseParameters { get; set; }

    [CliOption("--response-templates")]
    public IEnumerable<KeyValue>? ResponseTemplates { get; set; }

    [CliOption("--content-handling")]
    public string? ContentHandling { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}