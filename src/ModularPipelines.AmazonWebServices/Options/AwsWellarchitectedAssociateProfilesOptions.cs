using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "associate-profiles")]
public record AwsWellarchitectedAssociateProfilesOptions(
[property: CommandSwitch("--workload-id")] string WorkloadId,
[property: CommandSwitch("--profile-arns")] string[] ProfileArns
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}