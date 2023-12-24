using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "batch-stop-job-run")]
public record AwsGlueBatchStopJobRunOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--job-run-ids")] string[] JobRunIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}