using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-authorizer")]
public record AwsApigatewayUpdateAuthorizerOptions(
[property: CliOption("--rest-api-id")] string RestApiId,
[property: CliOption("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}