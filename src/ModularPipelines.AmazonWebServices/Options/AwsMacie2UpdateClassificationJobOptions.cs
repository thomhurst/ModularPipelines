using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "update-classification-job")]
public record AwsMacie2UpdateClassificationJobOptions(
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--job-status")] string JobStatus
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}