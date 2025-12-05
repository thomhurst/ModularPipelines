using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "associate-approval-rule-template-with-repository")]
public record AwsCodecommitAssociateApprovalRuleTemplateWithRepositoryOptions(
[property: CliOption("--approval-rule-template-name")] string ApprovalRuleTemplateName,
[property: CliOption("--repository-name")] string RepositoryName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}