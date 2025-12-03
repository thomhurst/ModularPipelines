using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "subscription", "show")]
public record AzAccountSubscriptionShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions;