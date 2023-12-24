using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "update-approval-rule-template-content")]
public record AwsCodecommitUpdateApprovalRuleTemplateContentOptions(
[property: CommandSwitch("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CommandSwitch("--new-rule-content")] string NewRuleContent
) : AwsOptions
{
    [CommandSwitch("--existing-rule-content-sha256")]
    public string? ExistingRuleContentSha256 { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}