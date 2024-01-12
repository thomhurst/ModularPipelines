using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "mv")]
public record GcloudStorageMvOptions(
[property: PositionalArgument] string Source,
[property: PositionalArgument] string Destination
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--all-versions")]
    public bool? AllVersions { get; set; }

    [BooleanCommandSwitch("--no-clobber")]
    public bool? NoClobber { get; set; }

    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [BooleanCommandSwitch("--daisy-chain")]
    public bool? DaisyChain { get; set; }

    [BooleanCommandSwitch("--do-not-decompress")]
    public bool? DoNotDecompress { get; set; }

    [CommandSwitch("--manifest-path")]
    public string? ManifestPath { get; set; }

    [BooleanCommandSwitch("--preserve-posix")]
    public bool? PreservePosix { get; set; }

    [BooleanCommandSwitch("--print-created-message")]
    public bool? PrintCreatedMessage { get; set; }

    [BooleanCommandSwitch("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [BooleanCommandSwitch("--skip-unsupported")]
    public bool? SkipUnsupported { get; set; }

    [CommandSwitch("--storage-class")]
    public string? StorageClass { get; set; }

    [CommandSwitch("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CommandSwitch("--[no-]preserve-acl")]
    public string[]? NoPreserveAcl { get; set; }

    [CommandSwitch("--gzip-in-flight")]
    public string[]? GzipInFlight { get; set; }

    [BooleanCommandSwitch("--gzip-in-flight-all")]
    public bool? GzipInFlightAll { get; set; }

    [CommandSwitch("--gzip-local")]
    public string[]? GzipLocal { get; set; }

    [BooleanCommandSwitch("--gzip-local-all")]
    public bool? GzipLocalAll { get; set; }

    [BooleanCommandSwitch("--ignore-symlinks")]
    public bool? IgnoreSymlinks { get; set; }

    [BooleanCommandSwitch("--preserve-symlinks")]
    public bool? PreserveSymlinks { get; set; }
}