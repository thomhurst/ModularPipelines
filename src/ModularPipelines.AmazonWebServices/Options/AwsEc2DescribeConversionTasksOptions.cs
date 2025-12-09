using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-conversion-tasks")]
public record AwsEc2DescribeConversionTasksOptions : AwsOptions
{
    [CliOption("--conversion-task-ids")]
    public string[]? ConversionTaskIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}