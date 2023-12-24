using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "get-findings-report-status")]
public record AwsInspector2GetFindingsReportStatusOptions : AwsOptions
{
    [CommandSwitch("--report-id")]
    public string? ReportId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}