using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "update-approval-rule-template-name")]
public record AwsCodecommitUpdateApprovalRuleTemplateNameOptions(
[property: CliOption("--old-approval-rule-template-name")] string OldApprovalRuleTemplateName,
[property: CliOption("--new-approval-rule-template-name")] string NewApprovalRuleTemplateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}