using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "start-instances")]
public record AwsEc2StartInstancesOptions(
[property: CommandSwitch("--instance-ids")] string[] InstanceIds
) : AwsOptions
{
    [CommandSwitch("--additional-info")]
    public string? AdditionalInfo { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}