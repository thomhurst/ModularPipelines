using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "wait", "fargate-profile-deleted")]
public record AwsEksWaitFargateProfileDeletedOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--fargate-profile-name")] string FargateProfileName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}