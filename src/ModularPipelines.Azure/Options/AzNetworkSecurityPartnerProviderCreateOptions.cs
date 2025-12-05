using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "security-partner-provider", "create")]
public record AzNetworkSecurityPartnerProviderCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vhub")]
    public string? Vhub { get; set; }
}