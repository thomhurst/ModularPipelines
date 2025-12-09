using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-documentation-version")]
public record AwsApigatewayUpdateDocumentationVersionOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--documentation-version")] string DocumentationVersion
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}