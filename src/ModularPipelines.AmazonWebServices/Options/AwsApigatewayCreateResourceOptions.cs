using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-resource")]
public record AwsApigatewayCreateResourceOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--parent-id")] string ParentId,
[property: CliOption("--path-part")] string PathPart
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}