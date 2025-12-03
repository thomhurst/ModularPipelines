using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "metrics", "update")]
public record AzStorageMetricsUpdateOptions(
[property: CliOption("--retention")] string Retention,
[property: CliOption("--services")] string Services
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--api")]
    public bool? Api { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliFlag("--hour")]
    public bool? Hour { get; set; }

    [CliFlag("--minute")]
    public bool? Minute { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}