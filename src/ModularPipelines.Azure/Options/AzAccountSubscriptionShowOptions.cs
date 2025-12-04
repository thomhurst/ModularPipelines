using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "subscription", "show")]
public record AzAccountSubscriptionShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;