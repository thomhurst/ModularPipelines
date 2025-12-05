using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "delete-fargate-profile")]
public record AwsEksDeleteFargateProfileOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--fargate-profile-name")] string FargateProfileName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}