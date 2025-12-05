using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "create-export")]
public record AwsLexv2ModelsCreateExportOptions(
[property: CliOption("--resource-specification")] string ResourceSpecification,
[property: CliOption("--file-format")] string FileFormat
) : AwsOptions
{
    [CliOption("--file-password")]
    public string? FilePassword { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}