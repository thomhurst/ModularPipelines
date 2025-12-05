using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "disassociate-approval-rule-template-from-repository")]
public record AwsCodecommitDisassociateApprovalRuleTemplateFromRepositoryOptions(
[property: CliOption("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}