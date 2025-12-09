using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "update-privacy-budget-template")]
public record AwsCleanroomsUpdatePrivacyBudgetTemplateOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--privacy-budget-template-identifier")] string PrivacyBudgetTemplateIdentifier,
[property: CliOption("--privacy-budget-type")] string PrivacyBudgetType
) : AwsOptions
{
    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}