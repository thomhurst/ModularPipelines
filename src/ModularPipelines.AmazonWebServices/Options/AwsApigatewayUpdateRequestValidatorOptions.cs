using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-request-validator")]
public record AwsApigatewayUpdateRequestValidatorOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--request-validator-id")] string RequestValidatorId
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}