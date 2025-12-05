using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "wait", "export-task-completed")]
public record AwsEc2WaitExportTaskCompletedOptions : AwsOptions
{
    [CliOption("--export-task-ids")]
    public string[]? ExportTaskIds { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}