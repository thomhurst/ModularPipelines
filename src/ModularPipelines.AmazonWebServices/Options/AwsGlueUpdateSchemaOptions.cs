using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-schema")]
public record AwsGlueUpdateSchemaOptions(
[property: CliOption("--schema-id")] string SchemaId
) : AwsOptions
{
    [CliOption("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CliOption("--compatibility")]
    public string? Compatibility { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}