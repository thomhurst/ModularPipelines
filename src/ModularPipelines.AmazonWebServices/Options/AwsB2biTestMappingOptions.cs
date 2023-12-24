using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "test-mapping")]
public record AwsB2biTestMappingOptions(
[property: CommandSwitch("--input-file-content")] string InputFileContent,
[property: CommandSwitch("--mapping-template")] string MappingTemplate,
[property: CommandSwitch("--file-format")] string FileFormat
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}