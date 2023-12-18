using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "add")]
public record AzAccountManagementGroupSubscriptionAddOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subscription")] string Subscription
) : AzOptions;