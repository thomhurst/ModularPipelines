using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "list-report-jobs")]
public record AwsBackupListReportJobsOptions : AwsOptions
{
    [CliOption("--by-report-plan-name")]
    public string? ByReportPlanName { get; set; }

    [CliOption("--by-creation-before")]
    public long? ByCreationBefore { get; set; }

    [CliOption("--by-creation-after")]
    public long? ByCreationAfter { get; set; }

    [CliOption("--by-status")]
    public string? ByStatus { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}