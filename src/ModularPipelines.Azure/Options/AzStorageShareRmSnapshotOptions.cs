using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "share-rm", "snapshot")]
public record AzStorageShareRmSnapshotOptions : AzOptions
{
    [CliOption("--access-tier")]
    public string? AccessTier { get; set; }

    [CliFlag("--enabled-protocols")]
    public bool? EnabledProtocols { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--quota")]
    public string? Quota { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--root-squash")]
    public string? RootSquash { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}