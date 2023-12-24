using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-create-case-options")]
public record AwsSupportDescribeCreateCaseOptionsOptions(
[property: CommandSwitch("--issue-type")] string IssueType,
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--language")] string Language,
[property: CommandSwitch("--category-code")] string CategoryCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}