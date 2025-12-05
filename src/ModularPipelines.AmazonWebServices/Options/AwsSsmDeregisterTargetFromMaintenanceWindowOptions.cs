using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "deregister-target-from-maintenance-window")]
public record AwsSsmDeregisterTargetFromMaintenanceWindowOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--window-target-id")] string WindowTargetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}