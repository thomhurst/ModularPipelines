using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-external-evaluation")]
public record AwsConfigservicePutExternalEvaluationOptions(
[property: CliOption("--config-rule-name")] string ConfigRuleName,
[property: CliOption("--external-evaluation")] string ExternalEvaluation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}