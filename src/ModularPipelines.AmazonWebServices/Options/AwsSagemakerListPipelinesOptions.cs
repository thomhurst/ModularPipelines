using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-pipelines")]
public record AwsSagemakerListPipelinesOptions : AwsOptions
{
    [CommandSwitch("--pipeline-name-prefix")]
    public string? PipelineNamePrefix { get; set; }

    [CommandSwitch("--created-after")]
    public long? CreatedAfter { get; set; }

    [CommandSwitch("--created-before")]
    public long? CreatedBefore { get; set; }

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