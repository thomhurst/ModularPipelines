using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-schema-by-definition")]
public record AwsGlueGetSchemaByDefinitionOptions(
[property: CommandSwitch("--schema-id")] string SchemaId,
[property: CommandSwitch("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}