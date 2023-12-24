using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "disassociate-profiles")]
public record AwsWellarchitectedDisassociateProfilesOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--profile-arns")] string[] ProfileArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}