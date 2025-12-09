using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "update-nodegroup-version")]
public record AwsEksUpdateNodegroupVersionOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CliOption("--release-version")]
    public string? ReleaseVersion { get; set; }

    [CliOption("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}