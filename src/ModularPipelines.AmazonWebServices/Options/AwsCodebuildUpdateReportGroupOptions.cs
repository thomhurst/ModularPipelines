using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "update-report-group")]
public record AwsCodebuildUpdateReportGroupOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--export-config")]
    public string? ExportConfig { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}