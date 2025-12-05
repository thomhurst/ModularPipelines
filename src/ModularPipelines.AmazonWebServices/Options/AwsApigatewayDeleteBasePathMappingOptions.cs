using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "delete-base-path-mapping")]
public record AwsApigatewayDeleteBasePathMappingOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--base-path")] string BasePath
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}