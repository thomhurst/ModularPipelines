using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "application", "create")]
public record AzHdinsightApplicationCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--script-action-name")] string ScriptActionName,
[property: CommandSwitch("--script-uri")] string ScriptUri
) : AzOptions
{
    [CommandSwitch("--access-mode")]
    public string? AccessMode { get; set; }

    [CommandSwitch("--destination-port")]
    public string? DestinationPort { get; set; }

    [BooleanCommandSwitch("--disable-gateway-auth")]
    public bool? DisableGatewayAuth { get; set; }

    [CommandSwitch("--edgenode-size")]
    public string? EdgenodeSize { get; set; }

    [CommandSwitch("--marketplace-id")]
    public string? MarketplaceId { get; set; }

    [BooleanCommandSwitch("--no-validation-timeout")]
    public bool? NoValidationTimeout { get; set; }

    [CommandSwitch("--script-parameters")]
    public string? ScriptParameters { get; set; }

    [CommandSwitch("--ssh-password")]
    public string? SshPassword { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CommandSwitch("--ssh-user")]
    public string? SshUser { get; set; }

    [CommandSwitch("--sub-domain-suffix")]
    public string? SubDomainSuffix { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}