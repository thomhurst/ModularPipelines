using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-approval-rule-template-description")]
public record AwsCodecommitUpdateApprovalRuleTemplateDescriptionOptions(
[property: CommandSwitch("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CommandSwitch("--approval-rule-template-description")] string ApprovalRuleTemplateDescription
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}