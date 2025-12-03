using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "query-schema-version-metadata")]
public record AwsGlueQuerySchemaVersionMetadataOptions : AwsOptions
{
    [CliOption("--schema-id")]
    public string? SchemaId { get; set; }

    [CliOption("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CliOption("--schema-version-id")]
    public string? SchemaVersionId { get; set; }

    [CliOption("--metadata-list")]
    public string[]? MetadataList { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}