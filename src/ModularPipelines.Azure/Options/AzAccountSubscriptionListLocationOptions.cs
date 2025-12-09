using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "subscription", "list-location")]
public record AzAccountSubscriptionListLocationOptions(
[property: CliOption("--id")] string Id
) : AzOptions;