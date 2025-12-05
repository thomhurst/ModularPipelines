using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "delete-privacy-budget-template")]
public record AwsCleanroomsDeletePrivacyBudgetTemplateOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--privacy-budget-template-identifier")] string PrivacyBudgetTemplateIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}