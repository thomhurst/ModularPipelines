using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-schema-version")]
public record AwsGlueGetSchemaVersionOptions : AwsOptions
{
    [CliOption("--schema-id")]
    public string? SchemaId { get; set; }

    [CliOption("--schema-version-id")]
    public string? SchemaVersionId { get; set; }

    [CliOption("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}