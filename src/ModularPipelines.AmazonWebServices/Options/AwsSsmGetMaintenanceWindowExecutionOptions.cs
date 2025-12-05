using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "get-maintenance-window-execution")]
public record AwsSsmGetMaintenanceWindowExecutionOptions(
[property: CliOption("--window-execution-id")] string WindowExecutionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}