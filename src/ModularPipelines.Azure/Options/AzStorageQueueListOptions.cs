using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "queue", "list")]
public record AzStorageQueueListOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--include-metadata")]
    public bool? IncludeMetadata { get; set; }

    [CliOption("--marker")]
    public string? Marker { get; set; }

    [CliOption("--num-results")]
    public string? NumResults { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--queue-endpoint")]
    public string? QueueEndpoint { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}