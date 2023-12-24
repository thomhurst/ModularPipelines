using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "schedule-run")]
public record AwsDevicefarmScheduleRunOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn,
[property: CommandSwitch("--test")] string Test
) : AwsOptions
{
    [CommandSwitch("--app-arn")]
    public string? AppArn { get; set; }

    [CommandSwitch("--device-pool-arn")]
    public string? DevicePoolArn { get; set; }

    [CommandSwitch("--device-selection-configuration")]
    public string? DeviceSelectionConfiguration { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--execution-configuration")]
    public string? ExecutionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}