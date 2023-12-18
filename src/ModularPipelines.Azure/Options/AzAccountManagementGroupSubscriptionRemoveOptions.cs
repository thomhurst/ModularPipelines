using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "remove")]
public record AzAccountManagementGroupSubscriptionRemoveOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subscription")] string Subscription
) : AzOptions;