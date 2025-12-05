using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("schemas", "update-schema")]
public record AwsSchemasUpdateSchemaOptions(
[property: CliOption("--registry-name")] string RegistryName,
[property: CliOption("--schema-name")] string SchemaName
) : AwsOptions
{
    [CliOption("--client-token-id")]
    public string? ClientTokenId { get; set; }

    [CliOption("--content")]
    public string? Content { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}