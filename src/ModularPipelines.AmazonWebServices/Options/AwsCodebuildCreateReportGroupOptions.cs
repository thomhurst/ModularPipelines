using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "create-report-group")]
public record AwsCodebuildCreateReportGroupOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--export-config")] string ExportConfig
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}