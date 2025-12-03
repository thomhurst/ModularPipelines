using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "file", "generate-sas", "(storage-preview", "extension)")]
public record AzStorageFileGenerateSasStoragePreviewExtensionOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--share-name")] string ShareName
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

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

    [CliFlag("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CliOption("--expiry")]
    public string? Expiry { get; set; }

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