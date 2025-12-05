using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "create-model")]
public record AwsApigatewayv2CreateModelOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--name")] string Name,
[property: CliOption("--schema")] string Schema
) : AwsOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}