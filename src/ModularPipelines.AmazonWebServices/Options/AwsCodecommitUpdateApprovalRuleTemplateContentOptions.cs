using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-approval-rule-template-content")]
public record AwsCodecommitUpdateApprovalRuleTemplateContentOptions(
[property: CliOption("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CliOption("--new-rule-content")] string NewRuleContent
) : AwsOptions
{
    [CliOption("--existing-rule-content-sha256")]
    public string? ExistingRuleContentSha256 { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}