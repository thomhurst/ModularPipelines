using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "accept-ownership-status")]
public record AzAccountAcceptOwnershipStatusOptions(
[property: CommandSwitch("--subscription-id")] string SubscriptionId
) : AzOptions
{
}