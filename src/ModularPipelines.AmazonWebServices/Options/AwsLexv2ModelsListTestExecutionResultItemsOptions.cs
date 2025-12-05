using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lexv2-models", "list-test-execution-result-items")]
public record AwsLexv2ModelsListTestExecutionResultItemsOptions(
[property: CliOption("--test-execution-id")] string TestExecutionId,
[property: CliOption("--result-filter-by")] string ResultFilterBy
) : AwsOptions
{
    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}