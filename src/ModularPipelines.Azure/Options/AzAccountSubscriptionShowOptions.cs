using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "subscription", "show")]
public record AzAccountSubscriptionShowOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;