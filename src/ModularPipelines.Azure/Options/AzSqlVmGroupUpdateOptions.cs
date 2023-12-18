using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "group", "update")]
public record AzSqlVmGroupUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--bootstrap-acc")]
    public string? BootstrapAcc { get; set; }

    [CommandSwitch("--cluster-subnet-type")]
    public string? ClusterSubnetType { get; set; }

    [CommandSwitch("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--fsw-path")]
    public string? FswPath { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--operator-acc")]
    public string? OperatorAcc { get; set; }

    [CommandSwitch("--ou-path")]
    public string? OuPath { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--sa-key")]
    public string? SaKey { get; set; }

    [CommandSwitch("--service-acc")]
    public string? ServiceAcc { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

