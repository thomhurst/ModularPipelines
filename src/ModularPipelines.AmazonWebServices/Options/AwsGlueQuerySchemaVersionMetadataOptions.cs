using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "query-schema-version-metadata")]
public record AwsGlueQuerySchemaVersionMetadataOptions : AwsOptions
{
    [CommandSwitch("--schema-id")]
    public string? SchemaId { get; set; }

    [CommandSwitch("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CommandSwitch("--schema-version-id")]
    public string? SchemaVersionId { get; set; }

    [CommandSwitch("--metadata-list")]
    public string[]? MetadataList { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}