using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search", "service", "update")]
public record AzSearchServiceUpdateOptions : AzOptions
{
    [CommandSwitch("--aad-auth-failure-mode")]
    public string? AadAuthFailureMode { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--auth-options")]
    public string? AuthOptions { get; set; }

    [BooleanCommandSwitch("--disable-local-auth")]
    public bool? DisableLocalAuth { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--identity-type")]
    public string? IdentityType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--ip-rules")]
    public string? IpRules { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--partition-count")]
    public int? PartitionCount { get; set; }

    [CommandSwitch("--public-access")]
    public string? PublicAccess { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--replica-count")]
    public int? ReplicaCount { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--semantic-search")]
    public string? SemanticSearch { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}