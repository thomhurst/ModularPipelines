using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-documentation-part")]
public record AwsApigatewayGetDocumentationPartOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--documentation-part-id")] string DocumentationPartId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}