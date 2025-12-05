using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-stage")]
public record AwsApigatewayDeleteStageOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--stage-name")] string StageName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}