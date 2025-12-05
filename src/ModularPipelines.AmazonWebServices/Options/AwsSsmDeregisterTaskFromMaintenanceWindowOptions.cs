using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "deregister-task-from-maintenance-window")]
public record AwsSsmDeregisterTaskFromMaintenanceWindowOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--window-task-id")] string WindowTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}