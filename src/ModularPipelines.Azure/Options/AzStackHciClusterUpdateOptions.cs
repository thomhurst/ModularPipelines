using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stack-hci", "cluster", "update")]
public record AzStackHciClusterUpdateOptions : AzOptions
{
    [CliOption("--aad-client-id")]
    public string? AadClientId { get; set; }

    [CliOption("--aad-tenant-id")]
    public string? AadTenantId { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--desired-properties")]
    public string? DesiredProperties { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

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
}