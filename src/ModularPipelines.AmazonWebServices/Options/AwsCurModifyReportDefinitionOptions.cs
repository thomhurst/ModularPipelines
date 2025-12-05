using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cur", "modify-report-definition")]
public record AwsCurModifyReportDefinitionOptions(
[property: CliOption("--report-name")] string ReportName,
[property: CliOption("--report-definition")] string ReportDefinition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}