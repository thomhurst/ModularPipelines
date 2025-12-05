using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "get-base-path-mapping")]
public record AwsApigatewayGetBasePathMappingOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--base-path")] string BasePath
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}