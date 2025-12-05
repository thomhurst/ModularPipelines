using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "poll-for-task")]
public record AwsDatapipelinePollForTaskOptions(
[property: CliOption("--worker-group")] string WorkerGroup
) : AwsOptions
{
    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--instance-identity")]
    public string? InstanceIdentity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}