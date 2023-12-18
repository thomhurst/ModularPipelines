using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "file", "generate-sas", "(storage-preview", "extension)")]
public record AzStorageFileGenerateSasStoragePreviewExtensionOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--share-name")] string ShareName
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

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

    [BooleanCommandSwitch("--disallow-trailing-dot")]
    public bool? DisallowTrailingDot { get; set; }

    [CommandSwitch("--expiry")]
    public string? Expiry { get; set; }

    [CommandSwitch("--https-only")]
    public string? HttpsOnly { get; set; }

    [CommandSwitch("--ip")]
    public string? Ip { get; set; }

    [CommandSwitch("--permissions")]
    public string? Permissions { get; set; }

    [CommandSwitch("--policy-name")]
    public string? PolicyName { get; set; }

    [CommandSwitch("--start")]
    public string? Start { get; set; }
}