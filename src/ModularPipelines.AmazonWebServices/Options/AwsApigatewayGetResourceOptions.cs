using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-resource")]
public record AwsApigatewayGetResourceOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--resource-id")] string ResourceId
) : AwsOptions
{
    [CliOption("--embed")]
    public string[]? Embed { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}