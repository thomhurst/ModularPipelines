using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("palo-alto", "cloudngfw", "local-rulestack", "list-app-id")]
public record AzPaloAltoCloudngfwLocalRulestackListAppIdOptions : AzOptions
{
    [CliOption("--app-id-version")]
    public string? AppIdVersion { get; set; }

    [CliOption("--app-prefix")]
    public string? AppPrefix { get; set; }

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