using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hdinsight", "application", "create")]
public record AzHdinsightApplicationCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--script-action-name")] string ScriptActionName,
[property: CliOption("--script-uri")] string ScriptUri
) : AzOptions
{
    [CliOption("--access-mode")]
    public string? AccessMode { get; set; }

    [CliOption("--destination-port")]
    public string? DestinationPort { get; set; }

    [CliFlag("--disable-gateway-auth")]
    public bool? DisableGatewayAuth { get; set; }

    [CliOption("--edgenode-size")]
    public string? EdgenodeSize { get; set; }

    [CliOption("--marketplace-id")]
    public string? MarketplaceId { get; set; }

    [CliFlag("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [CliOption("--script-parameters")]
    public string? ScriptParameters { get; set; }

    [CliOption("--ssh-password")]
    public string? SshPassword { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--ssh-user")]
    public string? SshUser { get; set; }

    [CliOption("--sub-domain-suffix")]
    public string? SubDomainSuffix { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}