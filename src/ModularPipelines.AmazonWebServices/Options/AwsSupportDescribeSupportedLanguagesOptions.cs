using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support", "describe-supported-languages")]
public record AwsSupportDescribeSupportedLanguagesOptions(
[property: CliOption("--issue-type")] string IssueType,
[property: CliOption("--service-code")] string ServiceCode,
[property: CliOption("--category-code")] string CategoryCode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}