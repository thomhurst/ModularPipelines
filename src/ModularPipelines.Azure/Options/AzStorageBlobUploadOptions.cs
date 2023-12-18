using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "upload")]
public record AzStorageBlobUploadOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--blob-url")]
    public string? BlobUrl { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--container-name")]
    public string? ContainerName { get; set; }

    [CommandSwitch("--content-cache")]
    public string? ContentCache { get; set; }

    [CommandSwitch("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CommandSwitch("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CommandSwitch("--content-language")]
    public string? ContentLanguage { get; set; }

    [CommandSwitch("--content-md5")]
    public string? ContentMd5 { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--data")]
    public string? Data { get; set; }

    [CommandSwitch("--encryption-scope")]
    public string? EncryptionScope { get; set; }

    [CommandSwitch("--file")]
    public string? File { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-modified-since")]
    public string? IfModifiedSince { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--if-unmodified-since")]
    public string? IfUnmodifiedSince { get; set; }

    [CommandSwitch("--lease-id")]
    public string? LeaseId { get; set; }

    [CommandSwitch("--length")]
    public string? Length { get; set; }

    [CommandSwitch("--max-connections")]
    public string? MaxConnections { get; set; }

    [CommandSwitch("--maxsize-condition")]
    public string? MaxsizeCondition { get; set; }

    [CommandSwitch("--metadata")]
    public string? Metadata { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-progress")]
    public bool? NoProgress { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--socket-timeout")]
    public string? SocketTimeout { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--tags-condition")]
    public string? TagsCondition { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [BooleanCommandSwitch("--validate-content")]
    public bool? ValidateContent { get; set; }
}

