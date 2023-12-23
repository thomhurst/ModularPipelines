using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "start-task-execution")]
public record AwsDatasyncStartTaskExecutionOptions(
[property: CommandSwitch("--task-arn")] string TaskArn
) : AwsOptions
{
    [CommandSwitch("--override-options")]
    public string? OverrideOptions { get; set; }

    [CommandSwitch("--includes")]
    public string[]? Includes { get; set; }

    [CommandSwitch("--excludes")]
    public string[]? Excludes { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--task-report-config")]
    public string? TaskReportConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}