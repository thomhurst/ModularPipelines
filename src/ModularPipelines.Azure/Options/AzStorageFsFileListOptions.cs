using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "fs", "file", "list")]
public record AzStorageFsFileListOptions(
[property: CommandSwitch("--file-system")] string FileSystem
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--exclude-dir")]
    public string? ExcludeDir { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--num-results")]
    public string? NumResults { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}