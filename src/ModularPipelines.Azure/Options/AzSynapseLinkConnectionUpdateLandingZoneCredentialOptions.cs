using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse", "link-connection", "update-landing-zone-credential")]
public record AzSynapseLinkConnectionUpdateLandingZoneCredentialOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--sas-token")] string SasToken,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions
{
}