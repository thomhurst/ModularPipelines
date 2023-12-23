using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-reserved-instances")]
public record AwsEc2ModifyReservedInstancesOptions(
[property: CommandSwitch("--reserved-instances-ids")] string[] ReservedInstancesIds,
[property: CommandSwitch("--target-configurations")] string[] TargetConfigurations
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}