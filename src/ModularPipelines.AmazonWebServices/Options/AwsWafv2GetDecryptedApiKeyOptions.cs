using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "get-decrypted-api-key")]
public record AwsWafv2GetDecryptedApiKeyOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--api-key")] string ApiKey
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}