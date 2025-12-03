using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "get-model-template")]
public record AwsApigatewayv2GetModelTemplateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--model-id")] string ModelId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}