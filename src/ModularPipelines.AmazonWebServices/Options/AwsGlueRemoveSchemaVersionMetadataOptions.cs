using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "remove-schema-version-metadata")]
public record AwsGlueRemoveSchemaVersionMetadataOptions(
[property: CliOption("--metadata-key-value")] string MetadataKeyValue
) : AwsOptions
{
    [CliOption("--schema-id")]
    public string? SchemaId { get; set; }

    [CliOption("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CliOption("--schema-version-id")]
    public string? SchemaVersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}