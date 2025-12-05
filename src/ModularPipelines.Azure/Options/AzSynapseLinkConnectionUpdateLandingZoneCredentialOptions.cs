using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse", "link-connection", "update-landing-zone-credential")]
public record AzSynapseLinkConnectionUpdateLandingZoneCredentialOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--sas-token")] string SasToken,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;