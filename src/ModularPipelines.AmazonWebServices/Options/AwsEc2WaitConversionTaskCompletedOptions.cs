using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "wait", "conversion-task-completed")]
public record AwsEc2WaitConversionTaskCompletedOptions : AwsOptions
{
    [CliOption("--conversion-task-ids")]
    public string[]? ConversionTaskIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}