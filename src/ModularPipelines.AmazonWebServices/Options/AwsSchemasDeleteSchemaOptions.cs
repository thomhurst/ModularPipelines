using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "delete-schema")]
public record AwsSchemasDeleteSchemaOptions(
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--schema-name")] string SchemaName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}