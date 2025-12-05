using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "schedule-run")]
public record AwsDevicefarmScheduleRunOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--test")] string Test
) : AwsOptions
{
    [CliOption("--app-arn")]
    public string? AppArn { get; set; }

    [CliOption("--device-pool-arn")]
    public string? DevicePoolArn { get; set; }

    [CliOption("--device-selection-configuration")]
    public string? DeviceSelectionConfiguration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--execution-configuration")]
    public string? ExecutionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}