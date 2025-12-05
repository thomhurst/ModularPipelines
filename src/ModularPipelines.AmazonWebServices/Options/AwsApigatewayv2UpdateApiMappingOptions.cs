using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "update-api-mapping")]
public record AwsApigatewayv2UpdateApiMappingOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--api-mapping-id")] string ApiMappingId,
[property: CliOption("--domain-name")] string DomainName
) : AwsOptions
{
    [CliOption("--api-mapping-key")]
    public string? ApiMappingKey { get; set; }

    [CliOption("--stage")]
    public string? Stage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}