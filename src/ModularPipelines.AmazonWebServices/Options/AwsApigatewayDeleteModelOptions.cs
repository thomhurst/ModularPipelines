using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-model")]
public record AwsApigatewayDeleteModelOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--model-name")] string ModelName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}