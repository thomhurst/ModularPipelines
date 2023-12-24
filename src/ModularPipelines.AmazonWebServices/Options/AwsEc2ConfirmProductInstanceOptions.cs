using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "confirm-product-instance")]
public record AwsEc2ConfirmProductInstanceOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--product-code")] string ProductCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}