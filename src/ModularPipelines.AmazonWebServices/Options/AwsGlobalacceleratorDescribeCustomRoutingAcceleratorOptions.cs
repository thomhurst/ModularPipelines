using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("globalaccelerator", "describe-custom-routing-accelerator")]
public record AwsGlobalacceleratorDescribeCustomRoutingAcceleratorOptions(
[property: CommandSwitch("--accelerator-arn")] string AcceleratorArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}