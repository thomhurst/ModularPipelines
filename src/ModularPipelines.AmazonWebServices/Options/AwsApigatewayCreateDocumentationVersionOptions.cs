using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-documentation-version")]
public record AwsApigatewayCreateDocumentationVersionOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--documentation-version")] string DocumentationVersion
) : AwsOptions
{
    [CliOption("--stage-name")]
    public string? StageName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}