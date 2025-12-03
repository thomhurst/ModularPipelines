using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-maintenance-window-task")]
public record AwsSsmGetMaintenanceWindowTaskOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--window-task-id")] string WindowTaskId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}