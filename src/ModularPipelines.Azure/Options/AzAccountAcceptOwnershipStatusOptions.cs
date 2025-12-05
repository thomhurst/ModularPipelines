using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("account", "accept-ownership-status")]
public record AzAccountAcceptOwnershipStatusOptions(
[property: CliOption("--subscription-id")] string SubscriptionId
) : AzOptions;