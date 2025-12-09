using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "test-mapping")]
public record AwsB2biTestMappingOptions(
[property: CliOption("--input-file-content")] string InputFileContent,
[property: CliOption("--mapping-template")] string MappingTemplate,
[property: CliOption("--file-format")] string FileFormat
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}