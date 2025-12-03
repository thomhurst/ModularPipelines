using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-documentation-part")]
public record AwsApigatewayCreateDocumentationPartOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--location")] string Location,
[property: CliOption("--properties")] string Properties
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}