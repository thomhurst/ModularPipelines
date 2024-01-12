using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "indexes", "create")]
public record GcloudAiIndexesCreateOptions(
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--metadata-file")] string MetadataFile
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--encryption-kms-key-name")]
    public string? EncryptionKmsKeyName { get; set; }

    [CommandSwitch("--index-update-method")]
    public string? IndexUpdateMethod { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--metadata-schema-uri")]
    public string? MetadataSchemaUri { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}