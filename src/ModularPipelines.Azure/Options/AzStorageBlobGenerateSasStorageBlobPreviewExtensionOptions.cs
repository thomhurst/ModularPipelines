using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "generate-sas", "(storage-blob-preview", "extension)")]
public record AzStorageBlobGenerateSasStorageBlobPreviewExtensionOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--as-user")]
    public bool? AsUser { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CommandSwitch("--cache-control")]
    public string? CacheControl { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--content-disposition")]
    public string? ContentDisposition { get; set; }

    [CommandSwitch("--content-encoding")]
    public string? ContentEncoding { get; set; }

    [CommandSwitch("--content-language")]
    public string? ContentLanguage { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--expiry")]
    public string? Expiry { get; set; }

    [BooleanCommandSwitch("--full-uri")]
    public bool? FullUri { get; set; }

    [CommandSwitch("--https-only")]
    public string? HttpsOnly { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--permissions")]
    public string? Permissions { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--snapshot")]
    public string? Snapshot { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }

    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }
}