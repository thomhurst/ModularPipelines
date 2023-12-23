using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "delete-schema-versions")]
public record AwsGlueDeleteSchemaVersionsOptions(
[property: CommandSwitch("--schema-id")] string SchemaId,
[property: CommandSwitch("--versions")] string Versions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}