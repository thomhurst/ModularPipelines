using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "objects", "update")]
public record GcloudStorageObjectsUpdateOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--all-versions")]
    public bool? AllVersions { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliOption("--[no-]event-based-hold")]
    public string[]? NoEventBasedHold { get; set; }

    [CliFlag("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [CliFlag("--recursive")]
    public bool? Recursive { get; set; }

    [CliOption("--storage-class")]
    public string? StorageClass { get; set; }

    [CliOption("--[no-]temporary-hold")]
    public string[]? NoTemporaryHold { get; set; }

    [CliOption("--acl-file")]
    public string? AclFile { get; set; }

    [CliOption("--add-acl-grant")]
    public string[]? AddAclGrant { get; set; }

    [CliOption("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CliOption("--[no-]preserve-acl")]
    public string[]? NoPreserveAcl { get; set; }

    [CliOption("--remove-acl-grant")]
    public string? RemoveAclGrant { get; set; }
}