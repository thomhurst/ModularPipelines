using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "wait", "conversion-task-cancelled")]
public record AwsEc2WaitConversionTaskCancelledOptions : AwsOptions
{
    [CommandSwitch("--conversion-task-ids")]
    public string[]? ConversionTaskIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}