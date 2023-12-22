using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "subscription", "enable")]
public record AzAccountSubscriptionEnableOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;