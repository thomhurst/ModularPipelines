using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "update-nodegroup-config")]
public record AwsEksUpdateNodegroupConfigOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--nodegroup-name")] string NodegroupName
) : AwsOptions
{
    [CommandSwitch("--labels")]
    public string? Labels { get; set; }

    [CommandSwitch("--taints")]
    public string? Taints { get; set; }

    [CommandSwitch("--scaling-config")]
    public string? ScalingConfig { get; set; }

    [CommandSwitch("--update-config")]
    public string? UpdateConfig { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}