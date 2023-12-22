using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "subscription", "show-sub-under-mg")]
public record AzAccountManagementGroupSubscriptionShowSubUnderMgOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;