using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("entityresolution", "delete-schema-mapping")]
public record AwsEntityresolutionDeleteSchemaMappingOptions(
[property: CommandSwitch("--schema-name")] string SchemaName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}