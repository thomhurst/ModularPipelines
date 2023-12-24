using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mturk", "delete-worker-block")]
public record AwsMturkDeleteWorkerBlockOptions(
[property: CommandSwitch("--worker-id")] string WorkerId
) : AwsOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}