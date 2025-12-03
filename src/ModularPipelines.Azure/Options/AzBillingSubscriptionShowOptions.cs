using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "subscription", "show")]
public record AzBillingSubscriptionShowOptions(
[property: CliOption("--account-name")] int AccountName
) : AzOptions;