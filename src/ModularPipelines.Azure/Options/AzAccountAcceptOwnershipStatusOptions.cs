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
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--owner-object-id")]
    public string? OwnerObjectId { get; set; }

    [CommandSwitch("--owner-spn")]
    public string? OwnerSpn { get; set; }

    [CommandSwitch("--owner-upn")]
    public string? OwnerUpn { get; set; }
}