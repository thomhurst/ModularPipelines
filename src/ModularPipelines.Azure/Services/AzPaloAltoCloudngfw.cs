using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("palo-alto")]
public class AzPaloAltoCloudngfw
{
    public AzPaloAltoCloudngfw(
        AzPaloAltoCloudngfwFirewall firewall,
        AzPaloAltoCloudngfwLocalRulestack localRulestack
    )
    {
        Firewall = firewall;
        LocalRulestack = localRulestack;
    }

    public AzPaloAltoCloudngfwFirewall Firewall { get; }

    public AzPaloAltoCloudngfwLocalRulestack LocalRulestack { get; }
}