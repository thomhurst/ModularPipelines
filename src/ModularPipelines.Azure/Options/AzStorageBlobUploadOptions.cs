using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "upload")]
public record AzStorageBlobUploadOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--blob-url")]
    public string? BlobUrl { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--content-cache")]
    public string? ContentCache { get; set; }

    [CliOption("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CliOption("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CliOption("--content-language")]
    public string? ContentLanguage { get; set; }

    [CliOption("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--data")]
    public string? Data { get; set; }

    [CliOption("--encryption-scope")]
    public string? EncryptionScope { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CliOption("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CliOption("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CliOption("--lease-id")]
    public string? LeaseId { get; set; }

    [CliOption("--length")]
    public string? Length { get; set; }

    [CliOption("--max-connections")]
    public string? MaxConnections { get; set; }

    [CliOption("--maxsize-condition")]
    public string? MaxsizeCondition { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-progress")]
    public bool? NoProgress { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--socket-timeout")]
    public string? SocketTimeout { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--tags-condition")]
    public string? TagsCondition { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliFlag("--validate-content")]
    public bool? ValidateContent { get; set; }
}