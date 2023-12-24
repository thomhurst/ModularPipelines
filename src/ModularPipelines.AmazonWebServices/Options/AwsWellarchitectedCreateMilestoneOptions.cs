using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-milestone")]
public record AwsWellarchitectedCreateMilestoneOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--milestone-name")] string MilestoneName
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}