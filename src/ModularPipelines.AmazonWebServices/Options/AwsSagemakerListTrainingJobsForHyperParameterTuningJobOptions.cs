using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-training-jobs-for-hyper-parameter-tuning-job")]
public record AwsSagemakerListTrainingJobsForHyperParameterTuningJobOptions(
[property: CommandSwitch("--hyper-parameter-tuning-job-name")] string HyperParameterTuningJobName
) : AwsOptions
{
    [CommandSwitch("--status-equals")]
    public string? StatusEquals { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}