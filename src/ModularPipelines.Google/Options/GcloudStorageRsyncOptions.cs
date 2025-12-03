using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "rsync")]
public record GcloudStorageRsyncOptions(
[property: CliArgument] string Source,
[property: CliArgument] string Destination
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliOption("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CliFlag("--checksums-only")]
    public bool? ChecksumsOnly { get; set; }

    [CliFlag("--no-clobber")]
    public bool? NoClobber { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliFlag("--delete-unmatched-destination-objects")]
    public bool? DeleteUnmatchedDestinationObjects { get; set; }

    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }

    [CliOption("--exclude")]
    public string[]? Exclude { get; set; }

    [CliOption("--gzip-in-flight")]
    public string[]? GzipInFlight { get; set; }

    [CliFlag("--gzip-in-flight-all")]
    public bool? GzipInFlightAll { get; set; }

    [CliFlag("--ignore-symlinks")]
    public bool? IgnoreSymlinks { get; set; }

    [CliFlag("--preserve-posix")]
    public bool? PreservePosix { get; set; }

    [CliFlag("--recursive")]
    public bool? Recursive { get; set; }

    [CliFlag("--skip-if-dest-has-newer-mtime")]
    public bool? SkipIfDestHasNewerMtime { get; set; }

    [CliFlag("--skip-unsupported")]
    public bool? SkipUnsupported { get; set; }
}