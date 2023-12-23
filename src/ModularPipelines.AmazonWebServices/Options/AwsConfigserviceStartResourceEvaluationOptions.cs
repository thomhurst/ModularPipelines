using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "start-resource-evaluation")]
public record AwsConfigserviceStartResourceEvaluationOptions(
[property: CommandSwitch("--resource-details")] string ResourceDetails,
[property: CommandSwitch("--evaluation-mode")] string EvaluationMode
) : AwsOptions
{
    [CommandSwitch("--evaluation-context")]
    public string? EvaluationContext { get; set; }

    [CommandSwitch("--evaluation-timeout")]
    public int? EvaluationTimeout { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}