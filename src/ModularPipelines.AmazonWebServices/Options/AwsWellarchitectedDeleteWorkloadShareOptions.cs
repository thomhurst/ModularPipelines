using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "delete-workload-share")]
public record AwsWellarchitectedDeleteWorkloadShareOptions(
[property: CommandSwitch("--share-id")] string ShareId,
[property: CommandSwitch("--workload-id")] string WorkloadId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}