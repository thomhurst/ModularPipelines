using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support", "describe-supported-languages")]
public record AwsSupportDescribeSupportedLanguagesOptions(
[property: CommandSwitch("--issue-type")] string IssueType,
[property: CommandSwitch("--service-code")] string ServiceCode,
[property: CommandSwitch("--category-code")] string CategoryCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}