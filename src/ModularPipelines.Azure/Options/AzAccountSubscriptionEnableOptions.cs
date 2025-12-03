using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "subscription", "enable")]
public record AzAccountSubscriptionEnableOptions(
[property: CliOption("--id")] string Id
) : AzOptions;