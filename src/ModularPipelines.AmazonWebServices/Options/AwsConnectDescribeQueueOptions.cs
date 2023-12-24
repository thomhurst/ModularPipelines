using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-queue")]
public record AwsConnectDescribeQueueOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--queue-id")] string QueueId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}