using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "create-budget")]
public record AwsBudgetsCreateBudgetOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--budget")] string Budget
) : AwsOptions
{
    [CommandSwitch("--notifications-with-subscribers")]
    public string[]? NotificationsWithSubscribers { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}