using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-documentation-part")]
public record AwsApigatewayUpdateDocumentationPartOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--documentation-part-id")] string DocumentationPartId
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}