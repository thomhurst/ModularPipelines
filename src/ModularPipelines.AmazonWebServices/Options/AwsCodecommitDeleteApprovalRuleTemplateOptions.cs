using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "delete-approval-rule-template")]
public record AwsCodecommitDeleteApprovalRuleTemplateOptions(
[property: CommandSwitch("--approval-rule-template-name")] string ApprovalRuleTemplateName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}