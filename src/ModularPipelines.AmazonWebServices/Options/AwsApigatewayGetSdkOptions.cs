using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-sdk")]
public record AwsApigatewayGetSdkOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--sdk-type")] string SdkType
) : AwsOptions
{
    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }
}