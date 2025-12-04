using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "management-group", "subscription", "show-sub-under-mg")]
public record AzAccountManagementGroupSubscriptionShowSubUnderMgOptions(
[property: CliOption("--name")] string Name
) : AzOptions;