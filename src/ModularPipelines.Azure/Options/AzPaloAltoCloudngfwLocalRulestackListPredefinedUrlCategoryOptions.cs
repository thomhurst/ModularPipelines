using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "local-rulestack", "list-predefined-url-category")]
public record AzPaloAltoCloudngfwLocalRulestackListPredefinedUrlCategoryOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--local-rulestack-name")]
    public string? LocalRulestackName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--skip")]
    public string? Skip { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}