using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "get-collaboration-privacy-budget-template")]
public record AwsCleanroomsGetCollaborationPrivacyBudgetTemplateOptions(
[property: CliOption("--collaboration-identifier")] string CollaborationIdentifier,
[property: CliOption("--privacy-budget-template-identifier")] string PrivacyBudgetTemplateIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}