using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "vm", "group", "update")]
public record AzSqlVmGroupUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--bootstrap-acc")]
    public string? BootstrapAcc { get; set; }

    [CliOption("--cluster-subnet-type")]
    public string? ClusterSubnetType { get; set; }

    [CliOption("--domain-fqdn")]
    public string? DomainFqdn { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--fsw-path")]
    public string? FswPath { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--operator-acc")]
    public string? OperatorAcc { get; set; }

    [CliOption("--ou-path")]
    public string? OuPath { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sa-key")]
    public string? SaKey { get; set; }

    [CliOption("--service-acc")]
    public string? ServiceAcc { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}