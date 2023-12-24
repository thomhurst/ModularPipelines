using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "set-status")]
public record AwsDatapipelineSetStatusOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId,
[property: CommandSwitch("--object-ids")] string[] ObjectIds,
[property: CommandSwitch("--status")] string Status
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}