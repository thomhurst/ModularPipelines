using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "update-export")]
public record AwsLexv2ModelsUpdateExportOptions(
[property: CliOption("--export-id")] string ExportId
) : AwsOptions
{
    [CliOption("--file-password")]
    public string? FilePassword { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}