using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "update-map-run")]
public record AwsStepfunctionsUpdateMapRunOptions(
[property: CommandSwitch("--map-run-arn")] string MapRunArn
) : AwsOptions
{
    [CommandSwitch("--max-concurrency")]
    public int? MaxConcurrency { get; set; }

    [CommandSwitch("--tolerated-failure-percentage")]
    public float? ToleratedFailurePercentage { get; set; }

    [CommandSwitch("--tolerated-failure-count")]
    public long? ToleratedFailureCount { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}