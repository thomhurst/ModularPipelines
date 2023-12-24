using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-approval-rule-template-name")]
public record AwsCodecommitUpdateApprovalRuleTemplateNameOptions(
[property: CommandSwitch("--old-approval-rule-template-name")] string OldApprovalRuleTemplateName,
[property: CommandSwitch("--new-approval-rule-template-name")] string NewApprovalRuleTemplateName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}