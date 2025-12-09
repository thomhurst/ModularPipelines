using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-model")]
public record AwsApigatewayCreateModelOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--name")] string Name,
[property: CliOption("--content-type")] string ContentType
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schema")]
    public string? Schema { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}