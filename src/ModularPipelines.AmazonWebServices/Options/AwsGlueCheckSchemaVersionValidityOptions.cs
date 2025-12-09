using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "check-schema-version-validity")]
public record AwsGlueCheckSchemaVersionValidityOptions(
[property: CliOption("--data-format")] string DataFormat,
[property: CliOption("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}