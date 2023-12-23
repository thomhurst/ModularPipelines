using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "create-action")]
public record AwsSagemakerCreateActionOptions(
[property: CommandSwitch("--action-name")] string ActionName,
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--action-type")] string ActionType
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--properties")]
    public IEnumerable<KeyValue>? Properties { get; set; }

    [CommandSwitch("--metadata-properties")]
    public string? MetadataProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}