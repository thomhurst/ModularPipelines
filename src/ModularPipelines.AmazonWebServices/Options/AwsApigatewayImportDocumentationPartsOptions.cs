using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "import-documentation-parts")]
public record AwsApigatewayImportDocumentationPartsOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--body")] string Body
) : AwsOptions
{
    [CliOption("--mode")]
    public string? Mode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}