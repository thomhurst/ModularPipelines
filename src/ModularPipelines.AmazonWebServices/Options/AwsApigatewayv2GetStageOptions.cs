using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "get-stage")]
public record AwsApigatewayv2GetStageOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--stage-name")] string StageName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}