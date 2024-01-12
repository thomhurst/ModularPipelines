using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "restore")]
public record GcloudStorageRestoreOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions
{
    [BooleanCommandSwitch("--all-versions")]
    public bool? AllVersions { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--[no-]preserve-acl")]
    public string[]? NoPreserveAcl { get; set; }

    [BooleanCommandSwitch("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [BooleanCommandSwitch("--allow-overwrite")]
    public bool? AllowOverwrite { get; set; }

    [CommandSwitch("--deleted-after-time")]
    public string? DeletedAfterTime { get; set; }

    [CommandSwitch("--deleted-before-time")]
    public string? DeletedBeforeTime { get; set; }
}