using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "create-api-key")]
public record AwsWafv2CreateApiKeyOptions(
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--token-domains")] string[] TokenDomains
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}