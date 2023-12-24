using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "create-privacy-budget-template")]
public record AwsCleanroomsCreatePrivacyBudgetTemplateOptions(
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--auto-refresh")] string AutoRefresh,
[property: CommandSwitch("--privacy-budget-type")] string PrivacyBudgetType,
[property: CommandSwitch("--parameters")] string Parameters
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}