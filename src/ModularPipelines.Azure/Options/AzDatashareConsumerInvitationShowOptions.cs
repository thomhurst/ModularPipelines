using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "consumer-invitation", "show")]
public record AzDatashareConsumerInvitationShowOptions(
[property: CommandSwitch("--invitation-id")] string InvitationId,
[property: CommandSwitch("--location")] string Location
) : AzOptions;