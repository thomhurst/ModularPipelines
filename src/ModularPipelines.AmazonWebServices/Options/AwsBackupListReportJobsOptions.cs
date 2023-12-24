using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "list-report-jobs")]
public record AwsBackupListReportJobsOptions : AwsOptions
{
    [CommandSwitch("--by-report-plan-name")]
    public string? ByReportPlanName { get; set; }

    [CommandSwitch("--by-creation-before")]
    public long? ByCreationBefore { get; set; }

    [CommandSwitch("--by-creation-after")]
    public long? ByCreationAfter { get; set; }

    [CommandSwitch("--by-status")]
    public string? ByStatus { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}