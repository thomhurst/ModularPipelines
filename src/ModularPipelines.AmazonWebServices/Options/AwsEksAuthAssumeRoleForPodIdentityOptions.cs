using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks-auth", "assume-role-for-pod-identity")]
public record AwsEksAuthAssumeRoleForPodIdentityOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--token")] string Token
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}