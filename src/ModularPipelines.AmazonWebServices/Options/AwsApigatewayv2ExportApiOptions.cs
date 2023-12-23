using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "export-api")]
public record AwsApigatewayv2ExportApiOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--output-type")] string OutputType,
[property: CommandSwitch("--specification")] string Specification
) : AwsOptions
{
    [CommandSwitch("--export-version")]
    public string? ExportVersion { get; set; }

    [CommandSwitch("--stage-name")]
    public string? StageName { get; set; }
}