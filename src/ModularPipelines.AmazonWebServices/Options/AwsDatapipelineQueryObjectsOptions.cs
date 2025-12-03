using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "query-objects")]
public record AwsDatapipelineQueryObjectsOptions(
[property: CliOption("--pipeline-id")] string PipelineId,
[property: CliOption("--sphere")] string Sphere
) : AwsOptions
{
    [CliOption("--objects-query")]
    public string? ObjectsQuery { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}