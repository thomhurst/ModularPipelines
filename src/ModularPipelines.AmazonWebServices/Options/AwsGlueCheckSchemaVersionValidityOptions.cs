using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "check-schema-version-validity")]
public record AwsGlueCheckSchemaVersionValidityOptions(
[property: CommandSwitch("--data-format")] string DataFormat,
[property: CommandSwitch("--schema-definition")] string SchemaDefinition
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}