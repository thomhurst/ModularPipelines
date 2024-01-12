using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "images", "create")]
public record GcloudComputeImagesCreateOptions(
[property: PositionalArgument] string ImageName,
[property: CommandSwitch("--source-disk")] string SourceDisk,
[property: CommandSwitch("--source-image")] string SourceImage,
[property: CommandSwitch("--source-image-family")] string SourceImageFamily,
[property: CommandSwitch("--source-snapshot")] string SourceSnapshot,
[property: CommandSwitch("--source-uri")] string SourceUri
) : GcloudOptions
{
    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--csek-key-file")]
    public string? CsekKeyFile { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--family")]
    public string? Family { get; set; }

    [CommandSwitch("--forbidden-database-file")]
    public string[]? ForbiddenDatabaseFile { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--guest-os-features")]
    public string[]? GuestOsFeatures { get; set; }

    [CommandSwitch("--key-exchange-key-file")]
    public string[]? KeyExchangeKeyFile { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--licenses")]
    public string[]? Licenses { get; set; }

    [CommandSwitch("--platform-key-file")]
    public string? PlatformKeyFile { get; set; }

    [BooleanCommandSwitch("--require-csek-key-create")]
    public bool? RequireCsekKeyCreate { get; set; }

    [CommandSwitch("--signature-database-file")]
    public string[]? SignatureDatabaseFile { get; set; }

    [CommandSwitch("--source-disk-project")]
    public string? SourceDiskProject { get; set; }

    [CommandSwitch("--source-disk-zone")]
    public string? SourceDiskZone { get; set; }

    [CommandSwitch("--source-image-project")]
    public string? SourceImageProject { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}