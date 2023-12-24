using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-schema-versions-diff")]
public record AwsGlueGetSchemaVersionsDiffOptions(
[property: CommandSwitch("--schema-id")] string SchemaId,
[property: CommandSwitch("--first-schema-version-number")] string FirstSchemaVersionNumber,
[property: CommandSwitch("--second-schema-version-number")] string SecondSchemaVersionNumber,
[property: CommandSwitch("--schema-diff-type")] string SchemaDiffType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}