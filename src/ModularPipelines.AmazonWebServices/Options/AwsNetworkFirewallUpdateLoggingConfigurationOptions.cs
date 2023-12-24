using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-firewall", "update-logging-configuration")]
public record AwsNetworkFirewallUpdateLoggingConfigurationOptions : AwsOptions
{
    [CommandSwitch("--firewall-arn")]
    public string? FirewallArn { get; set; }

    [CommandSwitch("--firewall-name")]
    public string? FirewallName { get; set; }

    [CommandSwitch("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}