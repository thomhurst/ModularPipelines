using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "describe-stack-instance")]
public record AwsCloudformationDescribeStackInstanceOptions(
[property: CommandSwitch("--stack-set-name")] string StackSetName,
[property: CommandSwitch("--stack-instance-account")] string StackInstanceAccount,
[property: CommandSwitch("--stack-instance-region")] string StackInstanceRegion
) : AwsOptions
{
    [CommandSwitch("--call-as")]
    public string? CallAs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}