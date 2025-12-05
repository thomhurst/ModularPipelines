using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "test-parsing")]
public record AwsB2biTestParsingOptions(
[property: CliOption("--input-file")] string InputFile,
[property: CliOption("--file-format")] string FileFormat,
[property: CliOption("--edi-type")] string EdiType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}