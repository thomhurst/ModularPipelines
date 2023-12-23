using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-task")]
public record AwsDatasyncUpdateTaskOptions(
[property: CommandSwitch("--task-arn")] string TaskArn
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--excludes")]
    public string[]? Excludes { get; set; }

    [CommandSwitch("--schedule")]
    public string? Schedule { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CommandSwitch("--includes")]
    public string[]? Includes { get; set; }

    [CommandSwitch("--task-report-config")]
    public string? TaskReportConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}