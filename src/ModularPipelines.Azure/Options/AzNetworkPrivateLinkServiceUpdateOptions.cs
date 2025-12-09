using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "private-link-service", "update")]
public record AzNetworkPrivateLinkServiceUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--auto-approval")]
    public string? AutoApproval { get; set; }

    [CliFlag("--enable-proxy-protocol")]
    public bool? EnableProxyProtocol { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fqdns")]
    public string? Fqdns { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lb-frontend-ip-configs")]
    public string? LbFrontendIpConfigs { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }
}