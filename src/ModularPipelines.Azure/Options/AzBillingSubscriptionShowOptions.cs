using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "subscription", "show")]
public record AzBillingSubscriptionShowOptions(
[property: CommandSwitch("--account-name")] int AccountName
) : AzOptions;