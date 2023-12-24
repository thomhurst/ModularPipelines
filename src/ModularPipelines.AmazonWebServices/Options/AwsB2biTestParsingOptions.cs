using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "test-parsing")]
public record AwsB2biTestParsingOptions(
[property: CommandSwitch("--input-file")] string InputFile,
[property: CommandSwitch("--file-format")] string FileFormat,
[property: CommandSwitch("--edi-type")] string EdiType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}