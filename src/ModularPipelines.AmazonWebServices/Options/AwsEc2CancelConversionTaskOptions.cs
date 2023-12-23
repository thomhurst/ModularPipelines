using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "cancel-conversion-task")]
public record AwsEc2CancelConversionTaskOptions(
[property: CommandSwitch("--conversion-task-id")] string ConversionTaskId
) : AwsOptions
{
    [CommandSwitch("--reason-message")]
    public string? ReasonMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}