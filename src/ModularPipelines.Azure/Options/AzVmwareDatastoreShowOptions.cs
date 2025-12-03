using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "datastore", "show")]
public record AzVmwareDatastoreShowOptions : AzOptions
{
    [CliOption("--cluster")]
    public string? Cluster { get; set; }

    [CliOption("--datastore-name")]
    public string? DatastoreName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}