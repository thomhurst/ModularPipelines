using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-fleet")]
public record AwsEc2CreateFleetOptions(
[property: CommandSwitch("--launch-template-configs")] string[] LaunchTemplateConfigs,
[property: CommandSwitch("--target-capacity-specification")] string TargetCapacitySpecification
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--spot-options")]
    public string? SpotOptions { get; set; }

    [CommandSwitch("--on-demand-options")]
    public string? OnDemandOptions { get; set; }

    [CommandSwitch("--excess-capacity-termination-policy")]
    public string? ExcessCapacityTerminationPolicy { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--valid-from")]
    public long? ValidFrom { get; set; }

    [CommandSwitch("--valid-until")]
    public long? ValidUntil { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}