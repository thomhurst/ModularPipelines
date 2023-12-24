using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-model-card-export-jobs")]
public record AwsSagemakerListModelCardExportJobsOptions(
[property: CommandSwitch("--model-card-name")] string ModelCardName
) : AwsOptions
{
    [CommandSwitch("--model-card-version")]
    public int? ModelCardVersion { get; set; }

    [CommandSwitch("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CommandSwitch("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CommandSwitch("--model-card-export-job-name-contains")]
    public string? ModelCardExportJobNameContains { get; set; }

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