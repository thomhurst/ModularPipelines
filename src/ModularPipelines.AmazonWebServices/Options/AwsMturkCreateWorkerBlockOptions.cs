using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "create-worker-block")]
public record AwsMturkCreateWorkerBlockOptions(
[property: CommandSwitch("--worker-id")] string WorkerId,
[property: CommandSwitch("--reason")] string Reason
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}