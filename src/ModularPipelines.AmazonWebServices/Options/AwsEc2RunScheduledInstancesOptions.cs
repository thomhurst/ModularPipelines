using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "run-scheduled-instances")]
public record AwsEc2RunScheduledInstancesOptions(
[property: CommandSwitch("--launch-specification")] string LaunchSpecification,
[property: CommandSwitch("--scheduled-instance-id")] string ScheduledInstanceId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}