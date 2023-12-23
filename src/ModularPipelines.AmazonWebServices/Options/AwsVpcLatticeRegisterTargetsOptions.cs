using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vpc-lattice", "register-targets")]
public record AwsVpcLatticeRegisterTargetsOptions(
[property: CommandSwitch("--target-group-identifier")] string TargetGroupIdentifier,
[property: CommandSwitch("--targets")] string[] Targets
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}