using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-schema-versions-diff")]
public record AwsGlueGetSchemaVersionsDiffOptions(
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--first-schema-version-number")] string FirstSchemaVersionNumber,
[property: CliOption("--second-schema-version-number")] string SecondSchemaVersionNumber,
[property: CliOption("--schema-diff-type")] string SchemaDiffType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}