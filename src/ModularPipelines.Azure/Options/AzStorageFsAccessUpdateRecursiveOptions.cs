using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "fs", "access", "update-recursive")]
public record AzStorageFsAccessUpdateRecursiveOptions(
[property: CliOption("--acl")] string Acl,
[property: CliOption("--file-system")] string FileSystem,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--batch-size")]
    public string? BatchSize { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--continuation")]
    public string? Continuation { get; set; }

    [CliFlag("--continue-on-failure")]
    public bool? ContinueOnFailure { get; set; }

    [CliOption("--max-batches")]
    public string? MaxBatches { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}