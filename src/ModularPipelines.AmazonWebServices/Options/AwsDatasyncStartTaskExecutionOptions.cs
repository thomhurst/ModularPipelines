using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "start-task-execution")]
public record AwsDatasyncStartTaskExecutionOptions(
[property: CliOption("--task-arn")] string TaskArn
) : AwsOptions
{
    [CliOption("--override-options")]
    public string? OverrideOptions { get; set; }

    [CliOption("--includes")]
    public string[]? Includes { get; set; }

    [CliOption("--excludes")]
    public string[]? Excludes { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--task-report-config")]
    public string? TaskReportConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}