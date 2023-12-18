using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "directory", "move")]
public record AzStorageBlobDirectoryMoveOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--destination-path")] string DestinationPath,
[property: CommandSwitch("--source-path")] string SourcePath
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--lease-id")]
    public string? LeaseId { get; set; }

    [CommandSwitch("--move-mode")]
    public string? MoveMode { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--source-if-match")]
    public string? SourceIfMatch { get; set; }

    [CommandSwitch("--source-if-modified-since")]
    public string? SourceIfModifiedSince { get; set; }

    [CommandSwitch("--source-if-none-match")]
    public string? SourceIfNoneMatch { get; set; }

    [CommandSwitch("--source-if-unmodified-since")]
    public string? SourceIfUnmodifiedSince { get; set; }

    [CommandSwitch("--source-lease-id")]
    public string? SourceLeaseId { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}

