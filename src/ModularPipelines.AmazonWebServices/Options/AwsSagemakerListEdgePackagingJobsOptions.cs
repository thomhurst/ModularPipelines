using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-edge-packaging-jobs")]
public record AwsSagemakerListEdgePackagingJobsOptions : AwsOptions
{
    [CommandSwitch("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CommandSwitch("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CommandSwitch("--last-modified-time-after")]
    public long? LastModifiedTimeAfter { get; set; }

    [CommandSwitch("--last-modified-time-before")]
    public long? LastModifiedTimeBefore { get; set; }

    [CommandSwitch("--name-contains")]
    public string? NameContains { get; set; }

    [CommandSwitch("--model-name-contains")]
    public string? ModelNameContains { get; set; }

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