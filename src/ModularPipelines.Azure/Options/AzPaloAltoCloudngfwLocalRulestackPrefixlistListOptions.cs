using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("palo-alto", "cloudngfw", "local-rulestack", "prefixlist", "list")]
public record AzPaloAltoCloudngfwLocalRulestackPrefixlistListOptions(
[property: CliOption("--local-rulestack-name")] string LocalRulestackName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}