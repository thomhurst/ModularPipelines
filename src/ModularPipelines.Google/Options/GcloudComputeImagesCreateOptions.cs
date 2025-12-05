using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "images", "create")]
public record GcloudComputeImagesCreateOptions(
[property: CliArgument] string ImageName,
[property: CliOption("--source-disk")] string SourceDisk,
[property: CliOption("--source-image")] string SourceImage,
[property: CliOption("--source-image-family")] string SourceImageFamily,
[property: CliOption("--source-snapshot")] string SourceSnapshot,
[property: CliOption("--source-uri")] string SourceUri
) : GcloudOptions
{
    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--family")]
    public string? Family { get; set; }

    [CliOption("--forbidden-database-file")]
    public string[]? ForbiddenDatabaseFile { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CliOption("--key-exchange-key-file")]
    public string[]? KeyExchangeKeyFile { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--licenses")]
    public string[]? Licenses { get; set; }

    [CliOption("--platform-key-file")]
    public string? PlatformKeyFile { get; set; }

    [CliFlag("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CliOption("--signature-database-file")]
    public string[]? SignatureDatabaseFile { get; set; }

    [CliOption("--source-disk-project")]
    public string? SourceDiskProject { get; set; }

    [CliOption("--source-disk-zone")]
    public string? SourceDiskZone { get; set; }

    [CliOption("--source-image-project")]
    public string? SourceImageProject { get; set; }

    [CliOption("--storage-location")]
    public string? StorageLocation { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}