using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-queue")]
public record AwsConnectDeleteQueueOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--queue-id")] string QueueId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}