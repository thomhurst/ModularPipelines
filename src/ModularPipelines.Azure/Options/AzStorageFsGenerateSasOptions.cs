using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "fs", "generate-sas")]
public record AzStorageFsGenerateSasOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliFlag("--as-user")]
    public bool? AsUser { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--cache-control")]
    public string? CacheControl { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CliOption("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CliOption("--content-language")]
    public string? ContentLanguage { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--encryption-scope")]
    public string? EncryptionScope { get; set; }

    [CliOption("--expiry")]
    public string? Expiry { get; set; }

    [CliFlag("--full-uri")]
    public bool? FullUri { get; set; }

    [CliOption("--https-only")]
    public string? HttpsOnly { get; set; }

    [CliOption("--ip")]
    public string? Ip { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--start")]
    public string? Start { get; set; }
}