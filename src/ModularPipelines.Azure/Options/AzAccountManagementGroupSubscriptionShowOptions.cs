using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "show")]
public record AzAccountManagementGroupSubscriptionShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subscription")] string Subscription
) : AzOptions
{
}

