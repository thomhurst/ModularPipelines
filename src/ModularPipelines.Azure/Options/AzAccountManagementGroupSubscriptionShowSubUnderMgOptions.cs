using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "show-sub-under-mg")]
public record AzAccountManagementGroupSubscriptionShowSubUnderMgOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}