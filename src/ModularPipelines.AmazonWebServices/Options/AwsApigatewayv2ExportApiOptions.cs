using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "export-api")]
public record AwsApigatewayv2ExportApiOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--output-type")] string OutputType,
[property: CliOption("--specification")] string Specification
) : AwsOptions
{
    [CliOption("--export-version")]
    public string? ExportVersion { get; set; }

    [CliOption("--stage-name")]
    public string? StageName { get; set; }
}