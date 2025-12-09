using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "import-api")]
public record AwsApigatewayv2ImportApiOptions(
[property: CliOption("--body")] string Body
) : AwsOptions
{
    [CliOption("--basepath")]
    public string? Basepath { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}