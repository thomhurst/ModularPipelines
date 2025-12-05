using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "perimeter", "profile", "access-rule", "update")]
public record AzNetworkPerimeterProfileAccessRuleUpdateOptions : AzOptions
{
    [CliOption("--access-rule-name")]
    public string? AccessRuleName { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--address-prefixes")]
    public string? AddressPrefixes { get; set; }

    [CliOption("--direction")]
    public string? Direction { get; set; }

    [CliOption("--email-addresses")]
    public string? EmailAddresses { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fqdn")]
    public string? Fqdn { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--perimeter-name")]
    public string? PerimeterName { get; set; }

    [CliOption("--phone-numbers")]
    public string? PhoneNumbers { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--subscriptions")]
    public string? Subscriptions { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}