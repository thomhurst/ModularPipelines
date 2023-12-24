using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "disassociate-approval-rule-template-from-repository")]
public record AwsCodecommitDisassociateApprovalRuleTemplateFromRepositoryOptions(
[property: CommandSwitch("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CommandSwitch("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}