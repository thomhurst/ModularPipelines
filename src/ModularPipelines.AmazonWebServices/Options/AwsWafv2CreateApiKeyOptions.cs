using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "create-api-key")]
public record AwsWafv2CreateApiKeyOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--token-domains")] string[] TokenDomains
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}