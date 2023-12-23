using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "wait", "fargate-profile-deleted")]
public record AwsEksWaitFargateProfileDeletedOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--fargate-profile-name")] string FargateProfileName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}