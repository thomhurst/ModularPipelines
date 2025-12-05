using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigatewayv2", "create-api-mapping")]
public record AwsApigatewayv2CreateApiMappingOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--stage")] string Stage
) : AwsOptions
{
    [CliOption("--api-mapping-key")]
    public string? ApiMappingKey { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}