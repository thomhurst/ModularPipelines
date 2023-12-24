using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "query-objects")]
public record AwsDatapipelineQueryObjectsOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId,
[property: CommandSwitch("--sphere")] string Sphere
) : AwsOptions
{
    [CommandSwitch("--objects-query")]
    public string? ObjectsQuery { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}