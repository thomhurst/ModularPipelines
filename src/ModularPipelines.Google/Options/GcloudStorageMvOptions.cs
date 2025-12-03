using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "mv")]
public record GcloudStorageMvOptions(
[property: CliArgument] string Source,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--all-versions")]
    public bool? AllVersions { get; set; }

    [CliFlag("--no-clobber")]
    public bool? NoClobber { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliFlag("--daisy-chain")]
    public bool? DaisyChain { get; set; }

    [CliFlag("--do-not-decompress")]
    public bool? DoNotDecompress { get; set; }

    [CliOption("--manifest-path")]
    public string? ManifestPath { get; set; }

    [CliFlag("--preserve-posix")]
    public bool? PreservePosix { get; set; }

    [CliFlag("--print-created-message")]
    public bool? PrintCreatedMessage { get; set; }

    [CliFlag("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [CliFlag("--skip-unsupported")]
    public bool? SkipUnsupported { get; set; }

    [CliOption("--storage-class")]
    public string? StorageClass { get; set; }

    [CliOption("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CliOption("--[no-]preserve-acl")]
    public string[]? NoPreserveAcl { get; set; }

    [CliOption("--gzip-in-flight")]
    public string[]? GzipInFlight { get; set; }

    [CliFlag("--gzip-in-flight-all")]
    public bool? GzipInFlightAll { get; set; }

    [CliOption("--gzip-local")]
    public string[]? GzipLocal { get; set; }

    [CliFlag("--gzip-local-all")]
    public bool? GzipLocalAll { get; set; }

    [CliFlag("--ignore-symlinks")]
    public bool? IgnoreSymlinks { get; set; }

    [CliFlag("--preserve-symlinks")]
    public bool? PreserveSymlinks { get; set; }
}