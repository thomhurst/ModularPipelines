using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-job")]
public record AwsGlueUpdateJobOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--job-update")] string JobUpdate
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}