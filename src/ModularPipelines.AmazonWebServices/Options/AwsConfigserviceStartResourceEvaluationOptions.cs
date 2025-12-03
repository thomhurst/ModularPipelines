using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "start-resource-evaluation")]
public record AwsConfigserviceStartResourceEvaluationOptions(
[property: CliOption("--resource-details")] string ResourceDetails,
[property: CliOption("--evaluation-mode")] string EvaluationMode
) : AwsOptions
{
    [CliOption("--evaluation-context")]
    public string? EvaluationContext { get; set; }

    [CliOption("--evaluation-timeout")]
    public int? EvaluationTimeout { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}