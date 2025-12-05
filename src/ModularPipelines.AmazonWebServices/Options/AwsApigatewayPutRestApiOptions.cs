using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "put-rest-api")]
public record AwsApigatewayPutRestApiOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--body")] string Body
) : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}