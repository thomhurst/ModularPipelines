using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "register-schema-version")]
public record AwsGlueRegisterSchemaVersionOptions(
[property: CliOption("--schema-id")] string SchemaId,
[property: CliOption("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}