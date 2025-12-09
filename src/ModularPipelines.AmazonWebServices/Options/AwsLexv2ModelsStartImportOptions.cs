using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "start-import")]
public record AwsLexv2ModelsStartImportOptions(
[property: CliOption("--import-id")] string ImportId,
[property: CliOption("--resource-specification")] string ResourceSpecification,
[property: CliOption("--merge-strategy")] string MergeStrategy
) : AwsOptions
{
    [CliOption("--file-password")]
    public string? FilePassword { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}