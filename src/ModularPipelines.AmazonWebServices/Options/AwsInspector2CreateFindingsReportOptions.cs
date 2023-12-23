using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "create-findings-report")]
public record AwsInspector2CreateFindingsReportOptions(
[property: CommandSwitch("--report-format")] string ReportFormat,
[property: CommandSwitch("--s3-destination")] string S3Destination
) : AwsOptions
{
    [CommandSwitch("--filter-criteria")]
    public string? FilterCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}