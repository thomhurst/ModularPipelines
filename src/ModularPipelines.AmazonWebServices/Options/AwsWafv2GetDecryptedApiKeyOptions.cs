using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wafv2", "get-decrypted-api-key")]
public record AwsWafv2GetDecryptedApiKeyOptions(
[property: CliOption("--scope")] string Scope,
[property: CliOption("--api-key")] string ApiKey
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}