using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "update-privacy-budget-template")]
public record AwsCleanroomsUpdatePrivacyBudgetTemplateOptions(
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--privacy-budget-template-identifier")] string PrivacyBudgetTemplateIdentifier,
[property: CommandSwitch("--privacy-budget-type")] string PrivacyBudgetType
) : AwsOptions
{
    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}