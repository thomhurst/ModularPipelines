using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("snow-device-management", "describe-execution")]
public record AwsSnowDeviceManagementDescribeExecutionOptions(
[property: CliOption("--managed-device-id")] string ManagedDeviceId,
[property: CliOption("--task-id")] string TaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}