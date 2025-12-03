using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "consumer-invitation", "reject-invitation")]
public record AzDatashareConsumerInvitationRejectInvitationOptions(
[property: CliOption("--invitation-id")] string InvitationId,
[property: CliOption("--location")] string Location
) : AzOptions;