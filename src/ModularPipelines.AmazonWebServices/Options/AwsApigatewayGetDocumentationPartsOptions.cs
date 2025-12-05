using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-documentation-parts")]
public record AwsApigatewayGetDocumentationPartsOptions(
[property: CliOption("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--name-query")]
    public string? NameQuery { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--location-status")]
    public string? LocationStatus { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}