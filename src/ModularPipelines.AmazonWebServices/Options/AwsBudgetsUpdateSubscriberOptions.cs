using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("budgets", "update-subscriber")]
public record AwsBudgetsUpdateSubscriberOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--budget-name")] string BudgetName,
[property: CommandSwitch("--notification")] string Notification,
[property: CommandSwitch("--old-subscriber")] string OldSubscriber,
[property: CommandSwitch("--new-subscriber")] string NewSubscriber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}