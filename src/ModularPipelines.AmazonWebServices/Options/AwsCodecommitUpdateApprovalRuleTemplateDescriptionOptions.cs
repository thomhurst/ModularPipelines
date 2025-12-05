using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-approval-rule-template-description")]
public record AwsCodecommitUpdateApprovalRuleTemplateDescriptionOptions(
[property: CliOption("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CliOption("--approval-rule-template-description")] string ApprovalRuleTemplateDescription
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}