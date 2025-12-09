using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-create-case-options")]
public record AwsSupportDescribeCreateCaseOptionsOptions(
[property: CliOption("--issue-type")] string IssueType,
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--language")] string Language,
[property: CliOption("--category-code")] string CategoryCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}