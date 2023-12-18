using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventgrid", "partner", "registration", "create")]
public record AzEventgridPartnerRegistrationCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--authorized-subscription-ids")]
    public string? AuthorizedSubscriptionIds { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}