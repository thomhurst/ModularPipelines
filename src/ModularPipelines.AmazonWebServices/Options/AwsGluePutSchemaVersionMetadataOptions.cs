using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "put-schema-version-metadata")]
public record AwsGluePutSchemaVersionMetadataOptions(
[property: CommandSwitch("--metadata-key-value")] string MetadataKeyValue
) : AwsOptions
{
    [CommandSwitch("--schema-id")]
    public string? SchemaId { get; set; }

    [CommandSwitch("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CommandSwitch("--schema-version-id")]
    public string? SchemaVersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}