using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-rest-api")]
public record AwsApigatewayUpdateRestApiOptions(
[property: CliOption("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}