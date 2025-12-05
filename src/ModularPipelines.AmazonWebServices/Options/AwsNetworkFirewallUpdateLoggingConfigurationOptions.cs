using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-firewall", "update-logging-configuration")]
public record AwsNetworkFirewallUpdateLoggingConfigurationOptions : AwsOptions
{
    [CliOption("--firewall-arn")]
    public string? FirewallArn { get; set; }

    [CliOption("--firewall-name")]
    public string? FirewallName { get; set; }

    [CliOption("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}