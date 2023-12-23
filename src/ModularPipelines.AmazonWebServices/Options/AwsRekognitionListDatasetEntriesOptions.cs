using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rekognition", "list-dataset-entries")]
public record AwsRekognitionListDatasetEntriesOptions(
[property: CommandSwitch("--dataset-arn")] string DatasetArn
) : AwsOptions
{
    [CommandSwitch("--contains-labels")]
    public string[]? ContainsLabels { get; set; }

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