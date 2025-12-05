using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "create-schema")]
public record AwsSchemasCreateSchemaOptions(
[property: CliOption("--content")] string Content,
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--schema-name")] string SchemaName,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}