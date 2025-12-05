using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "objects", "list")]
public record GcloudStorageObjectsListOptions(
[property: CliArgument] string Urls
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--exhaustive")]
    public bool? Exhaustive { get; set; }

    [CliFlag("--fetch-encrypted-object-hashes")]
    public bool? FetchEncryptedObjectHashes { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliFlag("--raw")]
    public bool? Raw { get; set; }

    [CliFlag("--soft-deleted")]
    public bool? SoftDeleted { get; set; }

    [CliFlag("--stat")]
    public bool? Stat { get; set; }
}