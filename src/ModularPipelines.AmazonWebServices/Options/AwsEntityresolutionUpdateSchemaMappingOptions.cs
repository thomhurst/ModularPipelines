using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "update-schema-mapping")]
public record AwsEntityresolutionUpdateSchemaMappingOptions(
[property: CommandSwitch("--mapped-input-fields")] string[] MappedInputFields,
[property: CommandSwitch("--schema-name")] string SchemaName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}