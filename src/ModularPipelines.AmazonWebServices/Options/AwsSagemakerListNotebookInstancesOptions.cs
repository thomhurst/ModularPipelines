using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-notebook-instances")]
public record AwsSagemakerListNotebookInstancesOptions : AwsOptions
{
    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--name-contains")]
    public string? NameContains { get; set; }

    [CommandSwitch("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CommandSwitch("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CommandSwitch("--last-modified-time-before")]
    public long? LastModifiedTimeBefore { get; set; }

    [CommandSwitch("--last-modified-time-after")]
    public long? LastModifiedTimeAfter { get; set; }

    [CommandSwitch("--status-equals")]
    public string? StatusEquals { get; set; }

    [CommandSwitch("--notebook-instance-lifecycle-config-name-contains")]
    public string? NotebookInstanceLifecycleConfigNameContains { get; set; }

    [CommandSwitch("--default-code-repository-contains")]
    public string? DefaultCodeRepositoryContains { get; set; }

    [CommandSwitch("--additional-code-repository-equals")]
    public string? AdditionalCodeRepositoryEquals { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}