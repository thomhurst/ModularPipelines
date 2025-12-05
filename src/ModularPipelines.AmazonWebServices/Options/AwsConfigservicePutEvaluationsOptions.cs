using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-evaluations")]
public record AwsConfigservicePutEvaluationsOptions(
[property: CliOption("--result-token")] string ResultToken
) : AwsOptions
{
    [CliOption("--evaluations")]
    public string[]? Evaluations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}