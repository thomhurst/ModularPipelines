using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "indexes", "create")]
public record GcloudAiIndexesCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--metadata-file")] string MetadataFile
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--encryption-kms-key-name")]
    public string? EncryptionKmsKeyName { get; set; }

    [CliOption("--index-update-method")]
    public string? IndexUpdateMethod { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--metadata-schema-uri")]
    public string? MetadataSchemaUri { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}