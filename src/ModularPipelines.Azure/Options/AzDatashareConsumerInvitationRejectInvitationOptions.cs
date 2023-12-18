using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "consumer-invitation", "reject-invitation")]
public record AzDatashareConsumerInvitationRejectInvitationOptions(
[property: CommandSwitch("--invitation-id")] string InvitationId,
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
}