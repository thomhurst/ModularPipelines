using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "reimport-api")]
public record AwsApigatewayv2ReimportApiOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--body")] string Body
) : AwsOptions
{
    [CliOption("--basepath")]
    public string? Basepath { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}