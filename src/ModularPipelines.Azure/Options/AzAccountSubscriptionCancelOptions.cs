using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "subscription", "cancel")]
public record AzAccountSubscriptionCancelOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}