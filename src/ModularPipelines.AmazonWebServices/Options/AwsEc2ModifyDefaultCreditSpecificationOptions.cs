using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-default-credit-specification")]
public record AwsEc2ModifyDefaultCreditSpecificationOptions(
[property: CommandSwitch("--instance-family")] string InstanceFamily,
[property: CommandSwitch("--cpu-credits")] string CpuCredits
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}