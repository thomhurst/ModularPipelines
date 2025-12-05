using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codecommit", "delete-approval-rule-template")]
public record AwsCodecommitDeleteApprovalRuleTemplateOptions(
[property: CliOption("--approval-rule-template-name")] string ApprovalRuleTemplateName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}