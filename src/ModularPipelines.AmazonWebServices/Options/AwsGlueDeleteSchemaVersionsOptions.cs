using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "delete-schema-versions")]
public record AwsGlueDeleteSchemaVersionsOptions(
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--versions")] string Versions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}