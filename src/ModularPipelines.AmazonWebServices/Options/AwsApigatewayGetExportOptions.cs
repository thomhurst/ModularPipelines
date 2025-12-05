using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-export")]
public record AwsApigatewayGetExportOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--stage-name")] string StageName,
[property: CliOption("--export-type")] string ExportType
) : AwsOptions
{
    [CliOption("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CliOption("--accepts")]
    public string? Accepts { get; set; }
}