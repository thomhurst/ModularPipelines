using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "ls")]
public record GcloudStorageLsOptions(
[property: PositionalArgument] string Path
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--all-versions")]
    public bool? AllVersions { get; set; }

    [BooleanCommandSwitch("--buckets")]
    public bool? Buckets { get; set; }

    [BooleanCommandSwitch("--etag")]
    public bool? Etag { get; set; }

    [BooleanCommandSwitch("--exhaustive")]
    public bool? Exhaustive { get; set; }

    [BooleanCommandSwitch("--fetch-encrypted-object-hashes")]
    public bool? FetchEncryptedObjectHashes { get; set; }

    [CommandSwitch("--format")]
    public new string? Format { get; set; }

    [CommandSwitch("--next-page-token")]
    public string? NextPageToken { get; set; }

    [BooleanCommandSwitch("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [BooleanCommandSwitch("--readable-sizes")]
    public bool? ReadableSizes { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--soft-deleted")]
    public bool? SoftDeleted { get; set; }

    [BooleanCommandSwitch("--full")]
    public bool? Full { get; set; }

    [BooleanCommandSwitch("--json")]
    public bool? Json { get; set; }

    [BooleanCommandSwitch("--long")]
    public bool? Long { get; set; }
}