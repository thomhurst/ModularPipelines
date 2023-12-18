using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto", "cloudngfw", "local-rulestack", "list-country")]
public record AzPaloAltoCloudngfwLocalRulestackListCountryOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--local-rulestack-name")]
    public string? LocalRulestackName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--skip")]
    public string? Skip { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--top")]
    public string? Top { get; set; }
}