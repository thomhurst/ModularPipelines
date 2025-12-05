using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "cors", "add")]
public record AzStorageCorsAddOptions(
[property: CliOption("--methods")] string Methods,
[property: CliOption("--origins")] string Origins,
[property: CliOption("--services")] string Services
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--allowed-headers")]
    public bool? AllowedHeaders { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--exposed-headers")]
    public string? ExposedHeaders { get; set; }

    [CliOption("--max-age")]
    public string? MaxAge { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}