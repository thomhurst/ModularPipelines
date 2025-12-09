using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "profile", "access-rule", "create")]
public record AzNetworkPerimeterProfileAccessRuleCreateOptions(
[property: CliOption("--access-rule-name")] string AccessRuleName,
[property: CliOption("--perimeter-name")] string PerimeterName,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--email-addresses")]
    public string? EmailAddresses { get; set; }

    [CliOption("--fqdn")]
    public string? Fqdn { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--phone-numbers")]
    public string? PhoneNumbers { get; set; }

    [CliOption("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}