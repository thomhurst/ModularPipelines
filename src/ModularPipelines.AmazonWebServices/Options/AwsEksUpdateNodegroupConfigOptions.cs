using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eks", "update-nodegroup-config")]
public record AwsEksUpdateNodegroupConfigOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliOption("--taints")]
    public string? Taints { get; set; }

    [CliOption("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CliOption("--update-config")]
    public string? UpdateConfig { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}