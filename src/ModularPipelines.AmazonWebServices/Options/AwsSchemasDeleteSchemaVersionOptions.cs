using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "delete-schema-version")]
public record AwsSchemasDeleteSchemaVersionOptions(
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--schema-version")] string SchemaVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}