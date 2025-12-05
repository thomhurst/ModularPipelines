using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elastictranscoder", "create-job")]
public record AwsElastictranscoderCreateJobOptions(
[property: CliOption("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CliOption("--input")]
    public string? Input { get; set; }

    [CliOption("--inputs")]
    public string[]? Inputs { get; set; }

    [CliOption("--outputs")]
    public string[]? Outputs { get; set; }

    [CliOption("--output-key-prefix")]
    public string? OutputKeyPrefix { get; set; }

    [CliOption("--playlists")]
    public string[]? Playlists { get; set; }

    [CliOption("--user-metadata")]
    public IEnumerable<KeyValue>? UserMetadata { get; set; }

    [CliOption("--job-output")]
    public string? JobOutput { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}