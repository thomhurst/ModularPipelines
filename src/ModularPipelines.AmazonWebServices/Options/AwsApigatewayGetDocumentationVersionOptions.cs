using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-documentation-version")]
public record AwsApigatewayGetDocumentationVersionOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--documentation-version")] string DocumentationVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}