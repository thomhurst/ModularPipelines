using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "perimeter", "profile", "access-rule", "create")]
public record AzNetworkPerimeterProfileAccessRuleCreateOptions(
[property: CommandSwitch("--access-rule-name")] string AccessRuleName,
[property: CommandSwitch("--perimeter-name")] string PerimeterName,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CommandSwitch("--direction")]
    public string? Direction { get; set; }

    [CommandSwitch("--email-addresses")]
    public string? EmailAddresses { get; set; }

    [CommandSwitch("--fqdn")]
    public string? Fqdn { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--phone-numbers")]
    public string? PhoneNumbers { get; set; }

    [CommandSwitch("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}