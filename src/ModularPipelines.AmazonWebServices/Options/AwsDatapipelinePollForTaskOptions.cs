using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "poll-for-task")]
public record AwsDatapipelinePollForTaskOptions(
[property: CommandSwitch("--worker-group")] string WorkerGroup
) : AwsOptions
{
    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--instance-identity")]
    public string? InstanceIdentity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}