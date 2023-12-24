using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-job-run")]
public record AwsGlueGetJobRunOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--run-id")] string RunId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}