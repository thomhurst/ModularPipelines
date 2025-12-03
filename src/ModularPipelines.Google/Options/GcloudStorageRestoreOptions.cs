using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "restore")]
public record GcloudStorageRestoreOptions(
[property: CliArgument] string Urls
) : GcloudOptions
{
    [CliFlag("--all-versions")]
    public bool? AllVersions { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--[no-]preserve-acl")]
    public string[]? NoPreserveAcl { get; set; }

    [CliFlag("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [CliFlag("--allow-overwrite")]
    public bool? AllowOverwrite { get; set; }

    [CliOption("--deleted-after-time")]
    public string? DeletedAfterTime { get; set; }

    [CliOption("--deleted-before-time")]
    public string? DeletedBeforeTime { get; set; }
}