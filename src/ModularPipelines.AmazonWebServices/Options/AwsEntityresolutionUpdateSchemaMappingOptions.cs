using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("entityresolution", "update-schema-mapping")]
public record AwsEntityresolutionUpdateSchemaMappingOptions(
[property: CliOption("--mapped-input-fields")] string[] MappedInputFields,
[property: CliOption("--schema-name")] string SchemaName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}