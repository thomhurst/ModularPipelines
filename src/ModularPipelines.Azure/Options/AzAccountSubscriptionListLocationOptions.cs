using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "subscription", "list-location")]
public record AzAccountSubscriptionListLocationOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions;