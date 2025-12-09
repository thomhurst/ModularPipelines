using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-fleet")]
public record AwsEc2CreateFleetOptions(
[property: CliOption("--launch-template-configs")] string[] LaunchTemplateConfigs,
[property: CliOption("--target-capacity-specification")] string TargetCapacitySpecification
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--spot-options")]
    public string? SpotOptions { get; set; }

    [CliOption("--on-demand-options")]
    public string? OnDemandOptions { get; set; }

    [CliOption("--excess-capacity-termination-policy")]
    public string? ExcessCapacityTerminationPolicy { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--valid-from")]
    public long? ValidFrom { get; set; }

    [CliOption("--valid-until")]
    public long? ValidUntil { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}