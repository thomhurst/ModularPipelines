using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks-auth", "assume-role-for-pod-identity")]
public record AwsEksAuthAssumeRoleForPodIdentityOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--token")] string Token
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}