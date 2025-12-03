using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-base-path-mapping")]
public record AwsApigatewayCreateBasePathMappingOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CliOption("--base-path")]
    public string? BasePath { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}