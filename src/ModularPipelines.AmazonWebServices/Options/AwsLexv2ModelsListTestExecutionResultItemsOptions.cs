using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lexv2-models", "list-test-execution-result-items")]
public record AwsLexv2ModelsListTestExecutionResultItemsOptions(
[property: CommandSwitch("--test-execution-id")] string TestExecutionId,
[property: CommandSwitch("--result-filter-by")] string ResultFilterBy
) : AwsOptions
{
    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}