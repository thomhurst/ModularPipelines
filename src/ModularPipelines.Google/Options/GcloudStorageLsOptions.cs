using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "ls")]
public record GcloudStorageLsOptions(
[property: CliArgument] string Path
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--all-versions")]
    public bool? AllVersions { get; set; }

    [CliFlag("--buckets")]
    public bool? Buckets { get; set; }

    [CliFlag("--etag")]
    public bool? Etag { get; set; }

    [CliFlag("--exhaustive")]
    public bool? Exhaustive { get; set; }

    [CliFlag("--fetch-encrypted-object-hashes")]
    public bool? FetchEncryptedObjectHashes { get; set; }

    [CliOption("--format")]
    public new string? Format { get; set; }

    [CliOption("--next-page-token")]
    public string? NextPageToken { get; set; }

    [CliFlag("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [CliFlag("--readable-sizes")]
    public bool? ReadableSizes { get; set; }

    [CliFlag("--recursive")]
    public bool? Recursive { get; set; }

    [CliFlag("--soft-deleted")]
    public bool? SoftDeleted { get; set; }

    [CliFlag("--full")]
    public bool? Full { get; set; }

    [CliFlag("--json")]
    public bool? Json { get; set; }

    [CliFlag("--long")]
    public bool? Long { get; set; }
}