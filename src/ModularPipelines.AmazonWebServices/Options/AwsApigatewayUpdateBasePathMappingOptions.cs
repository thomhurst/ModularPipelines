using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "update-base-path-mapping")]
public record AwsApigatewayUpdateBasePathMappingOptions(
[property: CliOption("--domain-name")] string DomainName,
[property: CliOption("--base-path")] string BasePath
) : AwsOptions
{
    [CliOption("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}