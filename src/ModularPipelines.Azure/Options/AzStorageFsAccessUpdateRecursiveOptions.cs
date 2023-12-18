using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "fs", "access", "update-recursive")]
public record AzStorageFsAccessUpdateRecursiveOptions(
[property: CommandSwitch("--acl")] string Acl,
[property: CommandSwitch("--file-system")] string FileSystem,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--batch-size")]
    public string? BatchSize { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--continuation")]
    public string? Continuation { get; set; }

    [BooleanCommandSwitch("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CommandSwitch("--max-batches")]
    public string? MaxBatches { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}