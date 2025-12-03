using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "logging", "update")]
public record AzStorageLoggingUpdateOptions(
[property: CliOption("--log")] string Log,
[property: CliOption("--retention")] string Retention,
[property: CliOption("--services")] string Services
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}