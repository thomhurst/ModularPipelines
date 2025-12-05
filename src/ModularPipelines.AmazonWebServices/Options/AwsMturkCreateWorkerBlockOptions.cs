using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mturk", "create-worker-block")]
public record AwsMturkCreateWorkerBlockOptions(
[property: CliOption("--worker-id")] string WorkerId,
[property: CliOption("--reason")] string Reason
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}