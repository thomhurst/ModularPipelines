using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "create-approval-rule-template")]
public record AwsCodecommitCreateApprovalRuleTemplateOptions(
[property: CliOption("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CliOption("--approval-rule-template-content")] string ApprovalRuleTemplateContent
) : AwsOptions
{
    [CliOption("--approval-rule-template-description")]
    public string? ApprovalRuleTemplateDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}