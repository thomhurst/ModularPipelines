using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "share-rm", "create")]
public record AzStorageShareRmCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--access-tier")]
    public string? AccessTier { get; set; }

    [CliFlag("--enabled-protocols")]
    public bool? EnabledProtocols { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--quota")]
    public string? Quota { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--root-squash")]
    public string? RootSquash { get; set; }
}