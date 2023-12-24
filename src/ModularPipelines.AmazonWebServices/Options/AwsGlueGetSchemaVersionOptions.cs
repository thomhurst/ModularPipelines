using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-schema-version")]
public record AwsGlueGetSchemaVersionOptions : AwsOptions
{
    [CommandSwitch("--schema-id")]
    public string? SchemaId { get; set; }

    [CommandSwitch("--schema-version-id")]
    public string? SchemaVersionId { get; set; }

    [CommandSwitch("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}