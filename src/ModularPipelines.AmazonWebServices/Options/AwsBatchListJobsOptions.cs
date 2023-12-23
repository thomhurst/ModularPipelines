using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "list-jobs")]
public record AwsBatchListJobsOptions : AwsOptions
{
    [CommandSwitch("--job-queue")]
    public string? JobQueue { get; set; }

    [CommandSwitch("--array-job-id")]
    public string? ArrayJobId { get; set; }

    [CommandSwitch("--multi-node-job-id")]
    public string? MultiNodeJobId { get; set; }

    [CommandSwitch("--job-status")]
    public string? JobStatus { get; set; }

    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}