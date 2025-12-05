using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-schema-by-definition")]
public record AwsGlueGetSchemaByDefinitionOptions(
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}