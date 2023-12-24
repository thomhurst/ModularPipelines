using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutvision", "list-dataset-entries")]
public record AwsLookoutvisionListDatasetEntriesOptions(
[property: CommandSwitch("--project-name")] string ProjectName,
[property: CommandSwitch("--dataset-type")] string DatasetType
) : AwsOptions
{
    [CommandSwitch("--anomaly-class")]
    public string? AnomalyClass { get; set; }

    [CommandSwitch("--before-creation-date")]
    public long? BeforeCreationDate { get; set; }

    [CommandSwitch("--after-creation-date")]
    public long? AfterCreationDate { get; set; }

    [CommandSwitch("--source-ref-contains")]
    public string? SourceRefContains { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}