using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-schema")]
public record AwsGlueUpdateSchemaOptions(
[property: CommandSwitch("--schema-id")] string SchemaId
) : AwsOptions
{
    [CommandSwitch("--schema-version-number")]
    public string? SchemaVersionNumber { get; set; }

    [CommandSwitch("--compatibility")]
    public string? Compatibility { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}