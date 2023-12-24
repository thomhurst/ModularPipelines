using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastictranscoder", "create-job")]
public record AwsElastictranscoderCreateJobOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CommandSwitch("--input")]
    public string? Input { get; set; }

    [CommandSwitch("--inputs")]
    public string[]? Inputs { get; set; }

    [CommandSwitch("--outputs")]
    public string[]? Outputs { get; set; }

    [CommandSwitch("--output-key-prefix")]
    public string? OutputKeyPrefix { get; set; }

    [CommandSwitch("--playlists")]
    public string[]? Playlists { get; set; }

    [CommandSwitch("--user-metadata")]
    public IEnumerable<KeyValue>? UserMetadata { get; set; }

    [CommandSwitch("--job-output")]
    public string? JobOutput { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}