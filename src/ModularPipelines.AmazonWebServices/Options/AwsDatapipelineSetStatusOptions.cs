using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "set-status")]
public record AwsDatapipelineSetStatusOptions(
[property: CliOption("--pipeline-id")] string PipelineId,
[property: CliOption("--object-ids")] string[] ObjectIds,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}