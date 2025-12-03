using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute-optimizer", "get-lambda-function-recommendations")]
public record AwsComputeOptimizerGetLambdaFunctionRecommendationsOptions : AwsOptions
{
    [CliOption("--function-arns")]
    public string[]? FunctionArns { get; set; }

    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}