using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "objects", "update")]
public record GcloudStorageObjectsUpdateOptions(
[property: PositionalArgument] string Url
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--all-versions")]
    public bool? AllVersions { get; set; }

    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CommandSwitch("--[no-]event-based-hold")]
    public string[]? NoEventBasedHold { get; set; }

    [BooleanCommandSwitch("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--storage-class")]
    public string? StorageClass { get; set; }

    [CommandSwitch("--[no-]temporary-hold")]
    public string[]? NoTemporaryHold { get; set; }

    [CommandSwitch("--acl-file")]
    public string? AclFile { get; set; }

    [CommandSwitch("--add-acl-grant")]
    public string[]? AddAclGrant { get; set; }

    [CommandSwitch("--canned-acl")]
    public string? CannedAcl { get; set; }

    [CommandSwitch("--[no-]preserve-acl")]
    public string[]? NoPreserveAcl { get; set; }

    [CommandSwitch("--remove-acl-grant")]
    public string? RemoveAclGrant { get; set; }
}