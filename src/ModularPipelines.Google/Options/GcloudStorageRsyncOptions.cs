using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "rsync")]
public record GcloudStorageRsyncOptions(
[property: PositionalArgument] string Source,
[property: PositionalArgument] string Destination
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CommandSwitch("--canned-acl")]
    public string? CannedAcl { get; set; }

    [BooleanCommandSwitch("--checksums-only")]
    public bool? ChecksumsOnly { get; set; }

    [BooleanCommandSwitch("--no-clobber")]
    public bool? NoClobber { get; set; }

    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [BooleanCommandSwitch("--delete-unmatched-destination-objects")]
    public bool? DeleteUnmatchedDestinationObjects { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [CommandSwitch("--exclude")]
    public string[]? Exclude { get; set; }

    [CommandSwitch("--gzip-in-flight")]
    public string[]? GzipInFlight { get; set; }

    [BooleanCommandSwitch("--gzip-in-flight-all")]
    public bool? GzipInFlightAll { get; set; }

    [BooleanCommandSwitch("--ignore-symlinks")]
    public bool? IgnoreSymlinks { get; set; }

    [BooleanCommandSwitch("--preserve-posix")]
    public bool? PreservePosix { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [BooleanCommandSwitch("--skip-if-dest-has-newer-mtime")]
    public bool? SkipIfDestHasNewerMtime { get; set; }

    [BooleanCommandSwitch("--skip-unsupported")]
    public bool? SkipUnsupported { get; set; }
}