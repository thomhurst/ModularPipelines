using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "update-nodegroup-version")]
public record AwsEksUpdateNodegroupVersionOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CommandSwitch("--release-version")]
    public string? ReleaseVersion { get; set; }

    [CommandSwitch("--launch-template")]
    public string? LaunchTemplate { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--kubernetes-version")]
    public string? KubernetesVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}