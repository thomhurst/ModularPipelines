using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "create-approval-rule-template")]
public record AwsCodecommitCreateApprovalRuleTemplateOptions(
[property: CommandSwitch("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CommandSwitch("--approval-rule-template-content")] string ApprovalRuleTemplateContent
) : AwsOptions
{
    [CommandSwitch("--approval-rule-template-description")]
    public string? ApprovalRuleTemplateDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}